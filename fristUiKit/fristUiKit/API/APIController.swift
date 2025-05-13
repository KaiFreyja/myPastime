//
//  APIController.swift
//  fristSwiftUI
//
//  Created by 尤凱 on 2025/4/10.
//

import Foundation

class APIController
{
    typealias APICallbackClosure = (APIResult) -> Void // 定義一個閉包型別
    
    func GetKaiTest(input: [String: Any], callBack: @escaping APICallbackClosure)
    {
        sendApi(url:"kai_test",input: input,callBack: callBack);
    }

    func GetRole(input: [String: Any], callBack: @escaping APICallbackClosure)
    {
        sendApi(url:"role",input: input,callBack: callBack);
    }
    
    func GetRoleResource(input:[String:Any],callBack: @escaping APICallbackClosure)
    {
        sendApi(url: "role_resource", input: input, callBack: callBack);
    }
    
    func GetGender(input: [String: Any], callBack: @escaping APICallbackClosure)
    {
        sendApi(url:"gender",input: input,callBack: callBack);
    }
    
    func GetProfession(input: [String: Any], callBack: @escaping APICallbackClosure)
    {
        sendApi(url:"profession",input: input,callBack: callBack);
    }
    
    func GetFgoRoleLevelAttr(input : [String:Any],callBack: @escaping APICallbackClosure)
    {
        sendApi(url:"level_attr" , input:input, callBack: callBack);
    }
    
    func GetFgoProfessionAtkRate(input : [String:Any],callBack: @escaping APICallbackClosure)
    {
        sendApi(url: "profession_atk_rate", input: input, callBack: callBack);
    }

    func GetFgoMapPos(input : [String:Any],callBack: @escaping APICallbackClosure)
    {
        sendApi(url: "map_pos", input: input, callBack: callBack);
    }

    func GetAiTalkGroup(input : [String:Any],callBack: @escaping APICallbackClosure)
    {
        sendApi(url: "talk_group", input: input, callBack: callBack);
    }

    func GetAiTalkContent(input : [String:Any],callBack: @escaping APICallbackClosure)
    {
        sendApi(url: "talk_content", input: input, callBack: callBack);
    }
    
    func AskAiTalk(input : [String:Any],callBack: @escaping APICallbackClosure)
    {
        postApi(url: "talk_group_ask", input: input, callBack: callBack);
    }
    
    func sendApi(url:String,input: [String: Any], callBack : @escaping APICallbackClosure)
    {
        print(input);
        
        var p : String = "";
        if(input.count > 0)
        {
            for key in input.keys
            {
                if(!p.isEmpty)
                {
                    p += "&";
                }
                else
                {
                    p += "?";
                }
                if let value = input[key] as? Int
                {
                    p += "\(key)=\(value)";
                }
                if let value = input[key] as? String
                {
                    p += "\(key)=\(value)";
                }
                if let value = input[key] as? Float
                {
                    p += "\(key)=\(value)";
                }
                if let value = input[key] as? Double
                {
                    p += "\(key)=\(value)";
                }
            }
        }
        
        
        let uri = config.API_DOMAIN + url + p;
        
        print(uri);
        
        fetchJSON(from: uri) { result in
            switch result {
            case .success(let json):
                var apiresult = SwiftAPIResult(data: json);
                print("\(apiresult)");
                callBack(apiresult);
            case .failure(let error):
                var errorresult = SwiftAPIResult();
                callBack(errorresult);
            }}
    }
    
    func postApi(url:String,input: [String: Any], callBack : @escaping APICallbackClosure)
    {
        print(input);
        
        let uri = config.API_DOMAIN + url;
        
        print(uri);
        
        postJSON(to: uri,body: input) { result in
            switch result {
            case .success(let json):
                var apiresult = SwiftAPIResult(data: json);
                print("\(apiresult)");
                callBack(apiresult);
            case .failure(let error):
                var errorresult = SwiftAPIResult();
                callBack(errorresult);
            }}
    }}
