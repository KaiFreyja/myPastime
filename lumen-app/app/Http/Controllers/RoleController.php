<?php
namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use App\Models\Role;

class RoleController extends Controller
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

    public function getAllRole(Request $request)
    {
        try
        {
            //$api_name = $this->getApiName($request->input("uri"));
            $api_name = $this->getApiName($request->path());
            $api_params = $this->getApiParams($request);
            $api_searchs = $this->getApiSearchs($request);
            $perPage = $request->input('per_page', 1000); // 預設每頁 1000 筆
            $page = $request->input('page', 1); // 預設第 1 頁

            $model = new Role();
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

    	public function getOneRole(Request $request,$id){
		try {
			if (!is_numeric($id) || $id == 0) {
				throw new \InvalidArgumentException('錯誤id:'.$id);
			}
			$api_name = $this->getApiName($request->path());
            $res = Role::find($id);
            
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
	public function postRole(Request $request){

		$api_name = $this->getApiName($request->path());
		$this->validate($request, [
		]);



		try {

			$query = new Role ; 
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
	public function putRole(Request $request,$id){
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
			$model = new Role ; 
			$pky = $model->primaryKey ;
			$selectItems = $request->selectItems;
			$sItems = json_decode($selectItems);
	
			$check_id = Role::where($pky , $id)->get();
			if(!$check_id || count($check_id) == 0){
				return response()->json(['message' => '此ID不存在。'], 422);
			}
			if($request->code){
				$check = Role::where($pky,$id)->first();
				if($check->code != $request->code){
				 $check_code = Role::where('code',$request->code)->get();
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
	public function deleteRole(Request $request,$id){
		$api_name = $this->getApiName($request->input("uri"));
		
		try {
			if (!is_numeric($id) || $id == 0) {
				throw new \InvalidArgumentException('錯誤id:'.$id);
			}
			$model = new Role ; 
			$pky = $model->primaryKey ;
			$query = Role::where($pky , $id)->delete();
			return response()->json([$api_name => $query], 200);
		} catch (\Exception $e) {
			return response()->json(['message' => $e->getMessage()], 409);
		}
	}


	public function psotUploadRoleImage(Request $request)
	{

		try {
			$this->validate($request, [
				'file' => 'required|image|mimes:jpeg,png,jpg|max:2048',
				'rid' => 'required|integer',
			]);

			$rid = $request->input("rid");

			// 如果驗證通過，程式會繼續執行到這裡
			if ($request->hasFile('file')) {

					$file = $request->file('file');
					// 設定檔案名稱        
					//$filename = $rid . '.' . $file->getClientOriginalExtension();
					$filename = $rid.'.'."png";
					// 設定儲存路徑
					$config = config('config');
					$subdir = "/images";
					$savePath = $config["upload_path"].'/role';
					// 建立資料夾/儲存檔案
					if($this->mkdirs($savePath)){
						$file->move($savePath, $filename);                        
						return response()->json(['message' => $savePath], 200);
					}else{
						$res = array(
							'error_code' => 'UI0002',
							'error_message' => 'mkdir is not fail.'
						);
						return response()->json([ "uploadImage" => $res ], 200);
					}
			}

			return response()->json(['message' => '沒有上傳任何檔案'], 400);

		} catch (\Illuminate\Validation\ValidationException $e) {
			return response()->json(['errors' => $e->errors()], 422);
		} catch (\Exception $e) {
			return response()->json(['message' => '伺服器錯誤：' . $e->getMessage()], 500);
		}
	}

	private function mkdirs($dir, $mode = 0777)
	{        
		if (is_dir($dir) || @mkdir($dir, $mode)) return true;
		if (!$this->mkdirs(dirname($dir), $mode)) return false;
		return @mkdir($dir, $mode);
	}
}
