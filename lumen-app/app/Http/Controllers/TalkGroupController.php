<?php
namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use App\Models\TalkGroup;
use App\Models\TalkContent;
use App\Http\Controllers\GeminiController;

class TalkGroupController extends Controller
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

    public function getAllTalkGroup(Request $request)
    {
        try
        {

            //$api_name = $this->getApiName($request->input("uri"));
            $api_name = $this->getApiName($request->path());
            $api_params = $this->getApiParams($request);
            $api_searchs = $this->getApiSearchs($request);
            $perPage = $request->input('per_page', 1000); // 預設每頁 1000 筆
            $page = $request->input('page', 1); // 預設第 1 頁
			$uid = @$api_searchs["uid"];

            $model = new TalkGroup();
            $pky = $model->primaryKey;
			$connection = $model->connection;
            $table = $model->table;
            $query = DB::connection($connection)->table($table);
            $query = $query->select($table.'.*');
			if(isset($uid))
			{
				$query = $query->where("uid","=",$uid);
			}
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

    public function getOneTalkGroup(Request $request,$id)
    {
		try {
			if (!is_numeric($id) || $id == 0) {
				throw new \InvalidArgumentException('錯誤id:'.$id);
			}
			$api_name = $this->getApiName($request->path());
            $res = TalkGroup::find($id);
            
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


    public function postTalkGroup(Request $request){

		$api_name = $this->getApiName($request->path());
		$this->validate($request, [
		]);



		try {

			$query = new TalkGroup ; 
			$pky = $query->primaryKey ;
            $query = $this->postInsertFieldQuery($request,$query) ; 

            if(isset($query->switch) && is_array($query->switch)) {
					$query->switch = $this->fillInTheValueFromBoolean(implode(',', $request->switch));	
				}
				else
				{					
					if(isset($request->switch))
					{
						$query->switch = $this->fillInTheValueFromBoolean($request->switch);	
					}						
				}

			$query->save();
			return response()->json([$api_name => $query, 'message' => 'CREATED'], 201);
		} catch (\Exception $e) {
			return response()->json(['message' => $e->getMessage()], 409);
		}
	}
	/**
	 * Put one Ebooks.
	 *
	 * @return Response
	 */
	public function putTalkGroup(Request $request,$id){
		$api_name = $this->getApiName($request->path());
$this->validate($request, [
    'code' => 'integer',
    'name' => 'max:32',
    'seq' => 'integer',
]);



		try {
			if (!is_numeric($id) || $id == 0) {
				throw new \InvalidArgumentException('錯誤id:'.$id);
			}
			$model = new TalkGroup ; 
			$pky = $model->primaryKey ;
			$selectItems = $request->selectItems;
			$sItems = json_decode($selectItems);
	
			$check_id = TalkGroup::where($pky , $id)->get();
			if(!$check_id || count($check_id) == 0){
				return response()->json(['message' => '此ID不存在。'], 422);
			}
			if($request->code){
				$check = TalkGroup::where($pky,$id)->first();
				if($check->code != $request->code){
				 $check_code = TalkGroup::where('code',$request->code)->get();
				 if($check_code && count($check_code) > 0){
				  return response()->json(['message' => '此代碼已存在，請更改後重新修改'], 422);
				 }
				}
			   }

			$updateArr = array();
			$updateArr = $this->getPutUpdateArray($request,$model);
			if (isset($request->name) && !empty($request->name)) {				
				$updateArr["name"] = $request->name;
			}
			if (isset($request->seq) && !empty($request->seq)) {				
				$updateArr["seq"] = $request->seq;
			}
			if(isset($request->switch) && is_array($request->switch)) {
				$updateArr["switch"] = $this->fillInTheValueFromBoolean(implode(',', $request->switch));
			}
			else
			{		
					if(isset($request->switch))
					{
						$updateArr["switch"] = $this->fillInTheValueFromBoolean($request->switch);
					}		
			}

			if (isset($request->created_at) && !empty($request->created_at)) {				
				$updateArr["created_at"] = $request->created_at;
			}
			if (isset($request->updated_at) && !empty($request->updated_at)) {				
				$updateArr["updated_at"] = $request->updated_at;
			}
			$query = $model->where($model->primaryKey,$id) ; 
			$query = $query->update($updateArr);

			return response()->json([ $api_name => $query ,'message' => 'CREATED'], 201);
		} catch (\Exception $e) {
			return response()->json(['message' => $e->getMessage()], 409);
		}
	}
	/**
	 * Delete one CashAccount.
	 *
	 * @return Response
	 */
	public function deleteTalkGroup(Request $request,$id){
		$api_name = $this->getApiName($request->input("uri"));
		
		try {
			if (!is_numeric($id) || $id == 0) {
				throw new \InvalidArgumentException('錯誤id:'.$id);
			}
			$model = new TalkGroup ; 
			$pky = $model->primaryKey ;
			$query = TalkGroup::where($pky , $id)->delete();
			return response()->json([$api_name => $query], 200);
		} catch (\Exception $e) {
			return response()->json(['message' => $e->getMessage()], 409);
		}
	}

    
	public function postSendAsk(Request $request){
		try {
			$api_name = $this->getApiName($request->path());
			$uid = $request->input("uid");
			$tgid = $request->input("tgid");
			$ask = $request->input("ask");

			if(!isset($uid))
			{
				return response()->json(['message' => "uid不存在"], 409);
			}

			if(isset($tgid))
			{
				$model = TalkGroup::find($tgid);
				if($model == null)
				{
					return response()->json(['message' => "tgid ".$tgid."不存在"], 409);
				}
			}

			if(!isset($tgid))
			{
				$model = new TalkGroup;
				$model->uid = $uid;
				$model->name = substr($ask, 0, 30);
				$model->description = substr($ask, 0, 250);
				$model->save();
				$tgid = $model->tgid;
			}

			//取得數量設定seq;
			$model = new TalkContent;
			$connection = $model->connection;
			$table = $model->table;
			$query = DB::connection($connection)->table($table);
			$query = $query->where("tgid","=",$tgid);
			$total = $query->count();


			//儲存此次發問
			$model = new TalkContent;
			$model->tgid = $tgid;
			$model->content = $ask;
			$model->talker = "user";
			$model->seq = $total + 1;
			$model->save();


			//取得所有發問
			$connection = $model->connection;
			$table = $model->table;
			$query = DB::connection($connection)->table($table);
			$query = $query->where("tgid","=",$tgid);
			$query = $query->orderBy($table.".seq","asc");
			$res = $query->get();

			//發送API
			$result = app(GeminiController::class)->formalAsk($res);

			//儲存回應
			$model = new TalkContent;
			$model->tgid = $tgid;
			$model->content = $result;
			$model->talker = "model";
			$model->seq = $total + 2;
			$model->save();

			return response()->json([$api_name => $model], 200);
		} catch (\Exception $e) {
			return response()->json(['message' => $e->getMessage()], 409);
		}
	}
}
