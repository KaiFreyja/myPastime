//
//  RoleListController.swift
//  fristUiKit
//
//  Created by 尤凱 on 2025/4/21.
//

import UIKit

class RoleListViewController : UIViewController ,UITableViewDataSource,UITableViewDelegate{
    
    @IBOutlet weak var roleTableView: UITableView!
    
    var roles : [[String:Any]] = [];
    
    override func viewDidLoad() {
        super.viewDidLoad();
        
        view.backgroundColor = .white;
        setupTableView();
        loadRoleData();
    }
    
    func setupTableView()
    {
        roleTableView.dataSource = self;
        roleTableView.delegate = self;
        roleTableView.register(UITableViewCell.self,forCellReuseIdentifier: "RoleCell");
    }
    
    func loadRoleData()
    {
        var controller = APIController();
        controller.GetRole(input: [:]){result in
            var resultData = result.getData();
            
            if let json = resultData as? [String:Any]
            {
                if let role = json["role"] as? [String:Any]
                {
                    if let data = role["data"] as? [[String:Any]]
                    {
                        self.roles = data;
                        self.roleTableView.reloadData();
                    }
                }
            }
        }
    }
    
 
    override func prepare(for segue: UIStoryboardSegue, sender: Any?)
    {
        
        if segue.identifier == "showRoleContent" {
            if let detailViewController = segue.destination as? RoleContentViewController {
                // 假設 RoleContentViewController 有一個名為 roleInfo 的屬性
                let selectedIndexPath = roleTableView.indexPathForSelectedRow!
                let selectedRole = roles[selectedIndexPath.row]
                //let selectedRole = roles[0];
                detailViewController.roleInfo = selectedRole
                roleTableView.deselectRow(at: selectedIndexPath, animated: true)
            }}
    }
}

extension RoleListViewController {
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return roles.count // 返回資料的數量
    }

    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "RoleCell", for: indexPath) // 重用 cell
        
        var roleName = "";
        var url = "";
        let roleInfo = roles[indexPath.row];
        if let name = roleInfo["name"] as? String
        {
            roleName = name;
        }
        if let rid = roleInfo["rid"] as? Int
        {
            url = "\(config.IMAGE_DOMAIN)\(rid).png";
        }
        
        cell.textLabel?.text = roleName // 設定 cell 的文字
        cell.imageView?.image = nil // 清除之前的圖片，避免圖片錯亂
        cell.imageView?.frame = CGRect(x : 0,y:0,width: 40, height: 40);
        cell.imageView?.contentMode = .scaleAspectFit;
        
        if let url = URL(string: url) {
            // 非同步載入圖片
            DispatchQueue.global(qos: .userInitiated).async {
                if let imageData = try? Data(contentsOf: url), let image = UIImage(data: imageData) {
                    DispatchQueue.main.async {
                        cell.imageView?.image = image
                        cell.setNeedsLayout() // 觸發 Cell 重新佈局以顯示圖片
                    }
                }
            }
        }
        
        return cell
    }
}

// MARK: - UITableViewDelegate (UITableView 委任)

extension RoleListViewController {
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        let selectedRole = roles[indexPath.row]
        print("Selected role: \(selectedRole)")
        performSegue(withIdentifier: "showRoleContent", sender: selectedRole);
    }
}
