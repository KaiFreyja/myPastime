<?php
namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use App\Models\Profession;

class ProfessionController extends Controller
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

    public function getAllProfession(Request $request)
    {
        try
        {
            //$api_name = $this->getApiName($request->input("uri"));
            $api_name = $this->getApiName($request->path());
            $api_params = $this->getApiParams($request);
            $api_searchs = $this->getApiSearchs($request);
            $perPage = $request->input('per_page', 1000); // 預設每頁 1000 筆
            $page = $request->input('page', 1); // 預設第 1 頁

            $model = new Profession();
            $pky = $model->primaryKey;
            $connection = $model->connection;
            $table = $model->table;
            $query = DB::connection($connection)->table($table);
            $query = $query->select($table.'.*');
            $query = $query->where("switch","=","1");
            $query = $query->orderBy($table.".seq","asc") ;
            // 執行分頁
            $res = $query->paginate($perPage, ['*'], 'page', $page);
            
            return response()->json([$api_name => $res], 200);
        }
       	catch (\Exception $e) {
			return response()->json(['message' => $e->getMessage()], 409);
		}
    }

    	public function getOneProfession(Request $request,$id){
		try {
			if (!is_numeric($id) || $id == 0) {
				throw new \InvalidArgumentException('錯誤id:'.$id);
			}
			$api_name = $this->getApiName($request->path());
            $res = Profession::find($id);
            
			if($res){
				return response()->json([$api_name => $res] , 200);
			}
			else{
				return response()->json(['message' => '查無此資料!'], 404);
			}	
		} catch (\Exception $e) {
			return response()->json(['message' => $e->getMessage()], 409);
		}
	}
}
