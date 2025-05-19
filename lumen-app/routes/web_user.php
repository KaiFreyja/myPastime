<?php
            $apiName = "login";
            $objName = "Login";
            $router->post("login", $objName.'Controller@Login');
            $router->post("login_third", $objName.'Controller@LoginThird');
            
            
            $apiName = "user";
            $objName = "User";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);            
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);

            $apiName = "user_third_login";
            $objName = "UserThirdLogin";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);            
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);

            $apiName = "third_login_type";
            $objName = "ThirdLoginType";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);            
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);
?>