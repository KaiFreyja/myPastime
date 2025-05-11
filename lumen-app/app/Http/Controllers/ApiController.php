<?php
namespace App\Http\Controllers;

use Illuminate\Http\Request;

class ApiController extends Controller
{
    // 取得資料
    public function getData()
    {
        return response()->json([
            'status' => 'success',
            'message' => 'Data fetched successfully!',
            'data' => [
                'name' => 'Lumen API',
                'version' => '1.0.0'
            ]
        ]);
    }

    // 提交資料
    public function submitData(Request $request)
    {
        $data = $request->all();  // 取得 POST 資料

        // 回傳接收到的資料
        return response()->json([
            'status' => 'success',
            'message' => 'Data submitted successfully!',
            'received_data' => $data
        ]);
    }
}
?>