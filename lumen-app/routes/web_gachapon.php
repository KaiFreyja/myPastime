<?php
           /*
             *  轉蛋群組
             * */
            // Standard table api
            $apiName = "gachapon_group";
            $objName = "GachaponGroup";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);

            /*
             *  轉蛋細項
             * */
            // Standard table api
            $apiName = "gachapon_item";
            $objName = "GachaponItem";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);

            /*
             *  轉蛋類別
             * */
            // Standard table api
            $apiName = "gachapon_type";
            $objName = "GachaponType";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);

            /*
             *  轉蛋紀錄群組
             * */
            // Standard table api
            $apiName = "gachapon_history_group";
            $objName = "GachaponHistoryGroup";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);



           /*
             *  轉蛋紀錄
             * */
            // Standard table api
            $apiName = "gachapon_history";
            $objName = "GachaponHistory";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);


?>