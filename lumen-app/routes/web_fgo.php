<?php
            $apiName = "role";
            $objName = "Role";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);            
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);
            
            //角色資源
            $apiName = "role_resource";
            $objName = "RoleResource";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);            
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);
            //上傳角色圖片
            $router->post("upload_role_image", $objName.'Controller@psotUploadRoleImage');
 

            /*
             *  性別
             * */
            // Standard table api
            $apiName = "gender";
            $objName = "Gender";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);

            /*
             *  職業
             * */
            // Standard table api
            $apiName = "profession";
            $objName = "Profession";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);


            /*
             *  等級數值
             * */
            // Standard table api
            $apiName = "level_attr";
            $objName = "LevelAttr";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);

            //屬性
            $apiName = "feature";
            $objName = "Feature";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);


            //職階攻擊倍率
            $apiName = "profession_atk_rate";
            $objName = "ProfessionAtkRate";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);
            $router->post($apiName, $objName.'Controller@post'.$objName);
            //$router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            //$router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);
            $router->post($apiName."_save", $objName.'Controller@save'.$objName);

            //地圖座標
            $apiName = "map_pos";
            $objName = "MapPos";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);
            $router->post($apiName."_save", $objName.'Controller@save'.$objName);

            //對話組
            $apiName = "talk_group";
            $objName = "TalkGroup";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);
            $router->post($apiName."_ask", $objName.'Controller@postSendAsk');
            
            //對話內容
            $apiName = "talk_content";
            $objName = "TalkContent";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);

            //技能
            $apiName = "skill";
            $objName = "Skill";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);

            //技能等級
            $apiName = "skill_level";
            $objName = "SkillLevel";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);

            //技能等級效果
            $apiName = "skill_level_buff";
            $objName = "SkillLevelBuff";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);

            //效果
            $apiName = "buff";
            $objName = "Buff";
            $router->get($apiName, $objName.'Controller@getAll'.$objName);
            $router->get($apiName.'/{id}', $objName.'Controller@getOne'.$objName);
            $router->post($apiName, $objName.'Controller@post'.$objName);
            $router->put($apiName.'/{id}', $objName.'Controller@put'.$objName);
            $router->delete($apiName.'/{id}', $objName.'Controller@delete'.$objName);
?>