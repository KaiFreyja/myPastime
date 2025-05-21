<?php

/** @var \Laravel\Lumen\Routing\Router $router */

/*
|--------------------------------------------------------------------------
| Application Routes
|--------------------------------------------------------------------------
|
| Here is where you can register all of the routes for an application.
| It is a breeze. Simply tell Lumen the URIs it should respond to
| and give it the Closure to call when that URI is requested.
|
*/
$router->get('/img/role/{filename}', function ($filename) {
    $path = storage_path('role/' . $filename);

    if (!file_exists($path)) {
        abort(404);
    }

    return response()->file($path);
});

$router->get('/img/role/{rid}/{filename}', function ($rid, $filename) {
    $path = storage_path("role/{$rid}/{$filename}");

    if (!file_exists($path)) {
        abort(404);
    }

    return response()->file($path, [
        'Content-Type' => mime_content_type($path)
    ]);
});

$router->group(['prefix' => 'api'], function () use ($router) {
    $router->get('/', function () {
        return response()->json(['message' => 'API is working']);
        //return $router->app->version();
    });


    $router->get('/gemini', 'GeminiController@ask');
    /*
    $router->get('/hello', function () {
        return 'Hello from API!';
    });

    
    // 取得資料
    $router->get('/data', 'ApiController@getData');
    // 提交資料
    $router->post('/submit', 'ApiController@submitData');
    */
    $router_path = dirname(__FILE__); 
    include $router_path."/web_user.php";
    include $router_path."/web_fgo.php";
    include $router_path."/web_gachapon.php";
});