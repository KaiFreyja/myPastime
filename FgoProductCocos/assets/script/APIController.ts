import { Config } from "./Config";

export class APIController 
{
    public GetFgoGetRole(input : object, callBack : (result : APIResult) => void)
    {
        this.sendAPI("role", input, callBack);
    }

    public GetFgoRoleResource(input : object, callBack : (result : APIResult) => void)
    {
        this.sendAPI("role_resource", input, callBack);
    }

    public AskAiTalk(input : object, callBack : (result : APIResult) => void)
    {
        this.postAPI("talk_group_ask", input, callBack);
    }

    private sendAPI(url : string,data : object,callback : (result : APIResult) => void)
    {
        var params  : string = "";
        for(var key in data)
        {
            if(params .length == 0)
            {
                params  += "?";
            }
            else
            {
                params  += "&";
            }
            params  += key + "=" + data[key];
        }

        let path : string = Config.API_DOMAIN + url + params;
        fetch(path)
            .then(response => response.json())
            .then(data => {
                console.log('收到資料:', data);
                callback(new DataResult(data));
            })
            .catch(error => {
                console.error('API 錯誤:', error);
                callback(new DataResult(error));
            });
    }

    private postAPI(url : string,data : object,callback : (result : APIResult) => void)
    {
        let path : string = Config.API_DOMAIN + url;

        fetch(path, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data)
        })
        .then(res => res.json())
        .then(data => {
            console.log('收到資料:', data);
            callback(new DataResult(data));
        })
        .catch(error => {
            console.error('API 錯誤:', error);
            callback(new DataResult(error));
        });
    }

}

export interface APIResult
{
    getData() : object
}

class DataResult implements APIResult
{
    public data : object;
    constructor(data : object)
    {
        this.data = data;
    }
    public getData(): object {
        return this.data;
    }
}