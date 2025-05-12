<?php

return [
    'picture_domain' => '192.168.0.170',
    'picture_protocol' => 'http://',
    'path_data' => '/var/www/AdminApi/storage',
    'upload_path' => '/home/olt/product/myPastime/lumen-app/storage',
    'temp_path' => storage_path('temp'),
    'project_id' => 'teems',
#    'admin_account' => 'admin',
#    'admin_password' => 'tblnancrbctxckcii555',

    'point_cut' => 'cut',
    'point_plus' => 'plus',

    'pointEvent' => [
        'productBonusPoint' => 'product_bonus_point', // 產品贈送點數(通用型)
        'orderRefundePointDefault' => 'order_refunde_point_default', // 產品退還點數(預設)
        'orderRefundePointCustom' => 'order_refunde_point_custom', // 產品退還點數(自訂)
    ],
	
	'ENCODE_KEY' => 'hsvideo88888888\0',
    'HOST' => '172.30.20.52',
    'DOCKER_EXEC' => "docker exec php-fpm1 bash -c ",
    'DOCKER_EXEC2' => "docker exec php-fpm2 bash -c ",
    'M3U8_KEYFILE_TOOLS' => "/home/drm/bin/keyTools.php",
	'REMOTE_CONTROLLER_TOOLS'=> '/home/bin/fpmController',
	'REMOTE1_CONTROLLER_TOOLS'=> '/home/bin/remoteController',
	'REMOTE6_CONTROLLER_TOOLS'=> '/home/bin/remote6Controller',
    'Docker_Path' => '/home/olt/TronEduExamApi/docker',
	'VIDED_SERVER_DOMAIN'=> '172.30.20.52',
	'VIDED_SERVER_PORT'=> '22',
	'VIDED_SERVER_ACCOUNT'=> 'olt',
	'VIDED_SERVER_PASSWORD'=> 'rootroot'	,
	'server'=> '172.30.20.52',
	'port'=> '11211',
	'expire'=> '14400',
	'video_expire'=> '30',
	'MC_SERVER'=> '172.30.20.52',// MC 的快取時間
	'MC_PORT'=> '11211',// MC 的快取時間
	'MC_EXPIRE'=> '86400',// MC 的快取時間
	'TOKEN_EXPIRE'=> '86400',// TOKEN 的生命週期 , 三秒.
	'SID_EXPIRE'=> '21600', // SID 的生命週期 , 六小時.
	'ONLINE'=> '1800',// 在線人數時間援衝.
	'PUBLISH_EXPIRE'=> '86400',// 直播 的生命週期 
	'PLAY_EXPIRE'=> '86400', // 可以存取直播 的生命週期 
	'id'=> '456b7016a916a4b178dd72b947c152b7',
	'secret'=> '5217c28135c0f9b0a47d1d6f2266f952', 
	
    'super_meta_id' => '456b7016a916a4b178dd72b947c152b7',
    'super_meta_secret' => '5217c28135c0f9b0a47d1d6f2266f952',
    'super_meta_api_e' => 'http://172.30.20.52/AdminApi/api',
    'super_meta_api_j' => 'http://172.30.20.52/AdminApi/api',
    'super_meta_api_s' => 'http://172.30.20.52/AdminApi/api',
    'ecpay_order_prefix' => 'OLT',

    'userType' => [
        'guest' => 'guest', // 訪客
        'member' => 'member', // 會員
        'admin' => 'refunded' // 管理員
    ],

    'pushType' => [
        'message' => 'message', // 訊息
        'email' => 'email', // Email
        'message_email' => 'message_email' // 訊息+Email
    ],

    'refuned_point_days' => '180',
    'give_point_days' => '5'
];
