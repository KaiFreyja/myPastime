<?php
namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use App\Models\UserThirdLogin;
use App\Models\mUser;

class LoginController extends Controller
{
    /**
     * Create a new controller instance.
     *
     * @return void
     */
    public function __construct()
    {
        //
    }

    
    public function Login(Request $request)
    {
return "aa";
    }

    public function LoginThird(Request $request)
    {
        try
        {
            
            $api_name = $this->getApiName($request->path());
            $this->validate($request, [
                'oid' => 'required',
                'tltid' => 'required|integer'
            ]);

            $oid = $request->input('oid');
            $tltid = $request->input('tltid');

            $model = new UserThirdLogin();
            $pky = $model->primaryKey;
            $connection = $model->connection;
            $table = $model->table;
            
            $query = DB::connection($connection)->table($table);
            $query = $query->select($table.'.*');
            $query = $query->where("oid","=",$oid);
            $query = $query->where("tltid","=",$tltid);
            $query = $query->where("switch","=","1");
            $count = $query->count();

            if($count == 0)
            {          
                //註冊
                $model = new mUser();
                $model->name = $oid;
                $model->save();
                $uid = $model->uid;

                $model = new UserThirdLogin();
                $model->uid = $uid;
                $model->oid = $oid;
                $model->tltid = $tltid;
                $model->save();
            }

            $thirdLogin = $query->first();
            $uid = $thirdLogin->uid;

            $res = mUser::find($uid);

			return response()->json([$api_name => $res] , 200);
            
        } catch (\Exception $e) {
			return response()->json(['message' => $e->getMessage()], 409);
		}
    }
}
