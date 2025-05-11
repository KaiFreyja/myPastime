<?php
namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use App\Models\LevelAttr;

class LevelAttrController extends Controller
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

    public function getAllLevelAttr(Request $request)
    {
        try
        {
            //$api_name = $this->getApiName($request->input("uri"));
            $api_name = $this->getApiName($request->path());
            $api_params = $this->getApiParams($request);
            $api_searchs = $this->getApiSearchs($request);
            $perPage = $request->input('per_page', 1000); // 預設每頁 1000 筆
            $page = $request->input('page', 1); // 預設第 1 頁
            $rid = @$api_searchs["rid"];

            $model = new LevelAttr();
            $pky = $model->primaryKey;
            $connection = $model->connection;
            $table = $model->table;
            $query = DB::connection($connection)->table($table);
            $query = $query->select($table.'.*');
            $query = $query->where("switch","=","1");
            $query = $query->orderBy($table.".rid","desc")->orderBy($table.".seq","asc") ;
            if(isset($rid))
            {
                $query = $query->where("rid","=",$rid);
            }
            $res = $query->paginate($perPage, ['*'], 'page', $page);
            $total = $res->total();
            if (isset($rid) && $total == 0) 
			{ 
            	// 調用建立預設資料的函數
            	$this->defaultRoleLevelAttr($rid);
        	    $res = $query->paginate($perPage, ['*'], 'page', $page);
            }
            return response()->json([$api_name => $res], 200);
        }
       	catch (\Exception $e) {
			return response()->json(['message' => $e->getMessage()], 409);
		}
    }

	function defaultRoleLevelAttr($rid)
	{
		for($i = 0;$i < 120;$i++)
		{
			$levelAttr = new LevelAttr();
			$levelAttr->rid = $rid;
			$levelAttr->level = $i+1;
			$levelAttr->hp = ($i + 1) * 10;
			$levelAttr->atk = ($i + 1) * 10;
			$levelAttr->seq = $i + 1;
			$levelAttr->save();
		}
	}

    	public function getOneLevelAttr(Request $request,$id){
		try {
			if (!is_numeric($id) || $id == 0) {
				throw new \InvalidArgumentException('錯誤id:'.$id);
			}
			$api_name = $this->getApiName($request->path());
            $res = LevelAttr::find($id);
            
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


    	/**
	 * Post one Ebooks.
	 *
	 * @return Response
	 */
	public function postLevelAttr(Request $request){

		$api_name = $this->getApiName($request->input("uri"));
		$this->validate($request, [
		]);

		try {

			$query = new LevelAttr ; 
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
			$this->cleanApiAllCache($api_name);
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
	public function putLevelAttr(Request $request,$id)
	{
		$api_name = $this->getApiName($request->input("uri"));
		$this->validate($request, [
		]);

		try {
			if (!is_numeric($id) || $id == 0) {
				throw new \InvalidArgumentException('錯誤id:'.$id);
			}
			$model = new LevelAttr ; 
			$pky = $model->primaryKey ;
			$selectItems = $request->selectItems;
			$sItems = json_decode($selectItems);
	
			$check_id = LevelAttr::where($pky , $id)->get();
			if(!$check_id || count($check_id) == 0){
				return response()->json(['message' => '此ID不存在。'], 422);
			}

			$updateArr = array();
			$updateArr = $this->getPutUpdateArray($request,$model);
			if (isset($request->hp) && !empty($request->hp)) {				
				$updateArr["hp"] = $request->hp;
			}
			if (isset($request->atk) && !empty($request->atk)) {				
				$updateArr["atk"] = $request->atk;
			}
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
			$this->cleanApiAllCache($api_name);

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
	public function deleteLevelAttr(Request $request,$id){
		$api_name = $this->getApiName($request->input("uri"));
		
		try {
			if (!is_numeric($id) || $id == 0) {
				throw new \InvalidArgumentException('錯誤id:'.$id);
			}
			$model = new LevelAttr ; 
			$pky = $model->primaryKey ;
			$query = LevelAttr::where($pky , $id)->delete();
			$this->cleanApiAllCache($api_name);
			return response()->json([$api_name => $query], 200);
		} catch (\Exception $e) {
			return response()->json(['message' => $e->getMessage()], 409);
		}
	}

}
