<?php
namespace App\Http\Controllers;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use App\Models\Role;
use App\Models\GachaponGroup;
use App\Models\GachaponItem;
use App\Models\GachaponHistoryGroup;
use App\Models\GachaponHistory;
use App\Models\RarityWeights;
use App\Models\RarityWeightTemplates;

class GachaponController extends Controller
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

    //
    public function importAll(Request $request)
    {
        try
        {
            $api_name = $this->getApiName($request->path());

            //新增一個轉蛋group
            $model = new GachaponGroup();
            $model->rwtid = 1;
            $model->save();
            $ggid = $model->ggid;

            //匯入所有角色
            $model = new Role();
            $res = $model->get();

            for($i = 0;$i < Count($res); $i++)
            {
                $rid = $res[$i]->rid;
                $rarity = $res[$i]->rarity;

                $model = new GachaponItem();
                $model->gtid = 1;
                $model->gtid_value = $rid;
                $model->ggid = $ggid;
                $model->rarity = $rarity;
                $model->save();
            }

            $model = new GachaponItem();
            $query = $model->where("ggid","=",$ggid);
            $res = $query->get();

            return response()->json([$api_name => $res], 200);
        }
       	catch (\Exception $e) {
			return response()->json(['message' => $e->getMessage()], 409);
		}
    }

    public function gachaponOne(Request $request)
    {
        try
        {
            $api_name = $this->getApiName($request->path());
            $uid = $request->input("uid");
            $ggid = $request->input("ggid");

            $model = GachaponGroup::find($ggid);
            $rwtid = $model->rwtid;
            $model = RarityWeights::where("rwtid","=",$rwtid);
            $res = $model->get();

            //取得出貨稀有度
            $weight = 0;
            foreach($res as $item)
            {
                $weight += $item->weight;
            }
            $rand = rand(1,$weight);

            $target = 0;
            $rarity = 0;
            foreach($res as $item)
            {
                $target += $item->weight;
                if($target >= $rand)
                {
                    $rarity = $item->rarity;
                    break;
                }
            }


            //取得出貨卡片
            $query = GachaponItem::where("ggid","=",$ggid);
            $query = $query->where("rarity","=",$rarity);
            $items = $query->get();
            $weight = 0;
            foreach($items as $item)
            {
                $weight += $item->weight;
            }
            $rand = rand(1,$weight);
            $target = 0;
            foreach($items as $item)
            {
                $target += $item->weight;
                if($target >= $rand)
                {
                    $res = $item;
                    break;
                }
            }



            //紀錄
            $model = new GachaponHistoryGroup();
            $model->uid = $uid;
            $model->save();

            $ghgid = $model->ghgid;
            $model = new GachaponHistory();
            $model->ghgid = $ghgid;
            $model->giid = $res->giid;
            $model->save();

            
            $gtid = $res->gtid;
            $value = $res->gtid_value;
            switch($gtid)
            {
                case 1:
                    //英靈
                    return app(RoleController::class)->getOneRole($request,$value);
                    break;
                default:
                    break;
            }

            return response()->json([$api_name => $res], 200);    
        }
       	catch (\Exception $e) {
			return response()->json(['message' => $e->getMessage()], 409);
		}
    }

    public function gachaponTen(Request $request)
    {
        try
        {
            $api_name = $this->getApiName($request->path());
            $uid = $request->input("uid");
            $ggid = $request->input("ggid");

            $model = GachaponGroup::find($ggid);
            $rwtid = $model->rwtid;
            $model = RarityWeights::where("rwtid","=",$rwtid);
            $res = $model->get();

            //紀錄
            $model = new GachaponHistoryGroup();
            $model->uid = $uid;
            $model->save();
            $ghgid = $model->ghgid;

            $results = [];
            for($i = 0;$i < 10;$i++)
            {
                //取得出貨稀有度
                $weight = 0;
                foreach($res as $item)
                {
                    $weight += $item->weight;
                }
                $rand = rand(1,$weight);

                $target = 0;
                $rarity = 0;
                foreach($res as $item)
                {
                    $target += $item->weight;
                    if($target >= $rand)
                    {
                        $rarity = $item->rarity;
                        break;
                    }
                }

                //取得出貨卡片
                $query = GachaponItem::where("ggid","=",$ggid);
                $query = $query->where("rarity","=",$rarity);
                $items = $query->get();
                $weight = 0;
                foreach($items as $item)
                {
                    $weight += $item->weight;
                }
                $rand = rand(1,$weight);
                $target = 0;
                $targetItem;
                foreach($items as $item)
                {
                    $target += $item->weight;
                    if($target >= $rand)
                    {
                        $targetItem = $item;
                        break;
                    }
                }

                //紀錄卡片
                $model = new GachaponHistory();
                $model->ghgid = $ghgid;
                $model->giid = $targetItem->giid;
                $model->save();
            
                $gtid = $targetItem->gtid;
                $value = $targetItem->gtid_value;
                switch($gtid)
                {
                    case 1:
                        //英靈
                        $role = app(RoleController::class)->getOneRole($request,$value);
                        $data = $role->getData(true);
                        $results[] = $data[$api_name];
                        break;
                    default:
                        break;
                }
            }

            return response()->json([$api_name => $results], 200);    
        }
       	catch (\Exception $e) {
			return response()->json(['message' => $e->getMessage()], 409);
		}
    }
}