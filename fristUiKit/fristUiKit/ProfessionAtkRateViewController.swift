//
//  ProfessionAtkRateViewController.swift
//  fristUiKit
//
//  Created by 尤凱 on 2025/5/5.
//

import UIKit

class ProfessionAtkRateViewController : UIViewController {
    
    var numRows = 100
    var numCols = 30
    let cellWidth: CGFloat = 100
    let cellHeight: CGFloat = 40
    var profession : [[String:Any]] = [];
    
    override func viewDidLoad() {
        super.viewDidLoad()
        view.backgroundColor = .white
        
        var controller = APIController();
        controller.GetProfession(input: [:]){result in
            var resultData = result.getData();
            if let json = resultData as? [String:Any]
            {
                if let profession = json["profession"] as? [String:Any]
                {
                    if let data = profession["data"] as? [[String:Any]]
                    {
                        self.profession = data;
                        self.numRows = data.count + 1;
                        self.numCols = data.count + 1;
                        self.createTable();
                        self.showData();
                    }
                }
            }
                
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
            if let name = data["name"] as? String
            {
                setValue(row: i + 1, col: 0, value: name);
                setValue(row: 0, col: i + 1, value: name);
            }
        }
        
        var controller = APIController();
        controller.GetFgoProfessionAtkRate(input: [:]){result in
            var resultData = result.getData();
            if let json = resultData as? [String:Any]
            {
                if let par = json["profession_atk_rate"] as? [String:Any]
                {
                    if let data = par["data"] as? [[String:Any]]
                    {
                        for index in 0..<data.count
                        {
                            if let one = data[index] as? [String:Any]
                            {
                                var atk : Int = 0;
                                var def : Int = 0;
                                var rate : Int = 0;
                                
                                if let atk_pid = one["atk_pid"] as? Int
                                {
                                    atk = atk_pid;
                                }
                                if let def_pid = one["def_pid"] as? Int
                                {
                                    def = def_pid;
                                }
                                if let mrate = one["rate"] as? Int
                                {
                                    rate = mrate;
                                }

                                var selectRow : Int = 0;
                                var selectCol : Int = 0;
                             
                                for i in 0..<self.profession.count
                                {
                                    var data = self.profession[i];
                                    if let pid = data["pid"] as? Int
                                    {
                                        if(atk == pid)
                                        {
                                            selectRow = i;
                                        }
                                        if(def == pid)
                                        {
                                            selectCol = i;
                                        }
                                    }
                                }
                                
                                self.setValue(row: selectRow + 1, col: selectCol + 1, value: "\(rate)");
                            }
                        }
                    }
                }
            }
        };
    }
}
