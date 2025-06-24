//
//  ProfessionAtkRateViewController.swift
//  fristUiKit
//
//  Created by 尤凱 on 2025/5/5.
//

import UIKit
import SwiftyJSON

class ProfessionAtkRateViewController : UIViewController {
    
    var numRows = 100
    var numCols = 30
    let cellWidth: CGFloat = 100
    let cellHeight: CGFloat = 40
    var profession : [JSON] = [];
    
    override func viewDidLoad() {
        super.viewDidLoad()
        view.backgroundColor = .white
        
        var controller = APIController();
        controller.GetProfession(input: [:]){result in
            
            let json = JSON(result.getData());
            let data = json["profession"]["data"].arrayValue;
            self.profession = data;
            self.numRows = data.count + 1;
            self.numCols = data.count + 1;
            self.createTable();
            self.showData();
               
            };
        
    }
    var labels : [UILabel] = [];
    func createTable()
    {
        let scrollView = UIScrollView(frame: view.bounds)
        scrollView.autoresizingMask = [.flexibleWidth, .flexibleHeight]
        
        let contentWidth = CGFloat(numCols) * cellWidth
        let contentHeight = CGFloat(numRows) * cellHeight
        
        let contentView = UIView(frame: CGRect(x: 0, y: 0, width: contentWidth, height: contentHeight))
        scrollView.contentSize = contentView.bounds.size
        scrollView.addSubview(contentView)
        view.addSubview(scrollView)
        
        for row in 0..<numRows {
            for col in 0..<numCols {
                let label = UILabel(frame: CGRect(x: CGFloat(col) * cellWidth,
                                                  y: CGFloat(row) * cellHeight,
                                                  width: cellWidth,
                                                  height: cellHeight))
                labels.append(label);
                label.textColor = .black;
                label.text = "\(row),\(col)"
                label.textAlignment = .center
                label.font = .systemFont(ofSize: 14)
                label.adjustsFontSizeToFitWidth = true
                label.layer.borderWidth = 0.5
                label.layer.borderColor = UIColor.gray.cgColor
                contentView.addSubview(label)
            }
        }
    }
    
    func setValue(row :Int,col :Int,value :String)
    {
        labels[row * numCols + col].text = value;
    }
    
    func showData()
    {
        setValue(row: 0, col: 0, value: "攻擊方/守備方")
        for i in 0..<profession.count
        {
            var data = profession[i];
            let name = data["name"].stringValue;
            
            setValue(row: i + 1, col: 0, value: name);
            setValue(row: 0, col: i + 1, value: name);
            
        }
        
        var controller = APIController();
        controller.GetFgoProfessionAtkRate(input: [:]){result in
            
            let json = JSON(result.getData());
            let data = json["profession_atk_rate"]["data"];
            for index in 0..<data.count
            {
                let one = data[index];
                
                var atk : Int = 0;
                var def : Int = 0;
                var rate : Int = 0;
                
                let atk_pid = one["atk_pid"].intValue;
                atk = atk_pid;
                let def_pid = one["def_pid"].intValue;
                def = def_pid;
                let mrate = one["rate"].intValue
                rate = mrate;
                
                
                var selectRow : Int = 0;
                var selectCol : Int = 0;
                
                for i in 0..<self.profession.count
                {
                    var data = self.profession[i];
                    let pid = data["pid"].intValue;
                    
                    if(atk == pid)
                    {
                        selectRow = i;
                    }
                    if(def == pid)
                    {
                        selectCol = i;
                    }
                    
                }
                
                self.setValue(row: selectRow + 1, col: selectCol + 1, value: "\(rate)");
                
            }
        };
    }
}
