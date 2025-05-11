//
//  MapPosController.swift
//  fristUiKit
//
//  Created by 尤凱 on 2025/5/6.
//

import UIKit;
import SwiftyJSON;

class AiTalkController : UIViewController,UITableViewDataSource,UITableViewDelegate{
    @IBOutlet weak var left_menu: UITableView!
    @IBOutlet weak var content_menu: UITableView!
    @IBOutlet weak var edit_ask: UITextField!
    @IBOutlet weak var btn_ask: UIButton!
    
    var talkGroupArray : JSON = [];
    var talkContentArray : JSON = [];
    var selectLeftJson : JSON = [];
    
    override func viewDidLoad() {
        super.viewDidLoad();
        setupTableView();
        loadTalkGroup();
    }
    
    func setupTableView()
    {
        left_menu.dataSource = self;
        left_menu.delegate = self;
        left_menu.register(UITableViewCell.self, forCellReuseIdentifier: "left_menu_cell");
        
        content_menu.dataSource = self;
        content_menu.delegate = self;
        content_menu.register(UITableViewCell.self, forCellReuseIdentifier: "content_cell");
    }
    
    func loadTalkGroup()
    {
        var controller = APIController();
        controller.GetAiTalkGroup(input: ["uid":GlobalData.uid]){result in
            
            let json = JSON(result.getData());
            let data = json["talk_group"]["data"];
            self.talkGroupArray = data;
            print("load : \(data)");
            self.left_menu.reloadData();
        }
    }
    func loadContent(json : JSON)
    {
        selectLeftJson = json;
        
        var tgid = json["tgid"].stringValue;
        var controller = APIController();
        controller.GetAiTalkContent(input: ["uid":GlobalData.uid,"tgid":tgid]){result in
            
            let json = JSON(result.getData());
            let data = json["talk_content"]["data"];
            self.talkContentArray = data;
            self.content_menu.reloadData();
        }
    }
    
    @IBAction func addNewTalk(_ sender: Any) {
        selectLeftJson = [];
        talkContentArray = [];
        content_menu.reloadData();
    }
    
    @IBAction func onClickAskSend(_ sender: Any) {
        print("onClickAskSend : \(edit_ask.text)");
    
        if(edit_ask.text == "")
        {
            return;
        }
        
        var text = edit_ask.text;
        edit_ask.text = "";
        
        var json = JSON(["talker":"user","content":text]);
        var temp = talkContentArray.arrayValue;
        temp.append(json);
        talkContentArray = JSON(temp);
        content_menu.reloadData();
        
        var input : [String:Any] = [:];
        if(selectLeftJson != [])
        {
            var tgid = selectLeftJson["tgid"].stringValue;
           input = ["uid": GlobalData.uid,"tgid":tgid,"ask":text];
        }
        else
        {
            input = ["uid": GlobalData.uid,"ask":text];
        }
        
        let lastRow = talkContentArray.count - 1
        if lastRow >= 0 {
            let indexPath = IndexPath(row: lastRow, section: 0)
            content_menu.scrollToRow(at: indexPath, at: .bottom, animated: true)
        }
        
        var controller = APIController();
        controller.AskAiTalk(input: input){result in
            
            
            var json = JSON(result.getData());
            var data = json["talk_group_ask"];
            var temp = self.talkContentArray.arrayValue;
            temp.append(data);
            self.talkContentArray = JSON(temp);
            self.content_menu.reloadData();
        }
        
    }
}


extension AiTalkController {
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if(tableView == left_menu)
        {
            return talkGroupArray.count;
        }
        
        if(tableView == content_menu)
        {
            return talkContentArray.count;
        }
        
        
        return 0;// roles.count // 返回資料的數量
    }

    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        
        if(tableView == left_menu)
        {
            let cell = tableView.dequeueReusableCell(withIdentifier: "left_menu_cell", for: indexPath) // 重用 cell
            let json = talkGroupArray[indexPath.row];
            var name = json["name"].stringValue;
            
            print("name : \(name)");
            
            cell.textLabel?.text = name;
            
            return cell;
        }
        
        if(tableView == content_menu)
        {
            let cell = tableView.dequeueReusableCell(withIdentifier: "content_cell", for: indexPath) // 重用 cell
            let json = talkContentArray[indexPath.row];
            var talker = json["talker"].stringValue;
            var content = json["content"].stringValue;
            cell.textLabel?.text = "\(talker) : \n\(content)";
            cell.textLabel?.numberOfLines = 0;
            cell.textLabel?.lineBreakMode = .byWordWrapping;
            
            return cell;
        }
        
        
        return UITableViewCell(); // 重用 cell
    }
}

// MARK: - UITableViewDelegate (UITableView 委任)

extension AiTalkController {
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        
        if(tableView == left_menu)
        {
            let json = talkGroupArray[indexPath.row];
            print("last \(json)");
            loadContent(json: json);
        }
        
    }
}
