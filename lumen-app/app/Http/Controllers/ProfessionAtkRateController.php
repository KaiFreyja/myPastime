<?php
namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use App\Models\ProfessionAtkRate;

class ProfessionAtkRateController extends Controller
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

    public function getAllProfessionAtkRate(Request $request)
    {
        try
        {
            //$api_name = $this->getApiName($request->input("uri"));
            $api_name = $this->getApiName($request->path());
            $api_params = $this->getApiParams($request);
            $api_searchs = $this->getApiSearchs($request);
            $perPage = $request->input('per_page', 1000); // 預設每頁 1000 筆
            $page = $request->input('page', 1); // 預設第 1 頁

            $model = new ProfessionAtkRate();
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

    	public function getOneProfessionAtkRate(Request $request,$id){
		try {
			if (!is_numeric($id) || $id == 0) {
				throw new \InvalidArgumentException('錯誤id:'.$id);
			}
			$api_name = $this->getApiName($request->path());
            $res = ProfessionAtkRate::find($id);
            
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

	public function postProfessionAtkRate(Request $request){

		$api_name = $this->getApiName($request->path());
		$this->validate($request, [
		]);



		try {

			$query = new ProfessionAtkRate ; 
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

    public function saveProfessionAtkRate(Request $request)
    {
        try {
        $dataArray = $request->input('data');

        
        foreach ($dataArray as $item) {
            $atk_pid = $item['atk_pid'];
            $def_pid = $item['def_pid'];
            $rate = $item['rate'];

            // 如果你的資料表是空的，updateOrCreate 其實會 insert
            // 但如果沒設定 fillable，或 connection 問題，也會失敗
            
            $record = ProfessionAtkRate::firstOrNew([
                'atk_pid' => $atk_pid,
                'def_pid' => $def_pid,
            ]);
            $record->rate = $rate;
            $record->save();
        }

        return response()->json(['message' => '儲存成功'], 200);
    } catch (\Exception $e) {
        return response()->json(['message' => $e->getMessage()], 500);
    }

    }
}
