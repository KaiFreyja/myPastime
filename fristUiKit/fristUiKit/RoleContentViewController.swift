//
//  RoleContentViewController.swift
//  fristUiKit
//
//  Created by 尤凱 on 2025/4/21.
//

import UIKit
import DGCharts

class RoleContentViewController: UIViewController {
    @IBOutlet weak var imgRole: UIImageView!
    @IBOutlet weak var tvName: UILabel!
    @IBOutlet weak var tvDescription: UILabel!
    @IBOutlet weak var tvGender: UILabel!
    @IBOutlet weak var tvProfession: UILabel!
    
    @IBOutlet weak var lineView: LineChartView!
    
    @IBOutlet weak var tvLineNum: UILabel!
    
    var roleInfo : [String:Any]?
    
    override func viewDidLoad() {
        super.viewDidLoad();
        print("5555555\(roleInfo)");
        
        if let info = roleInfo as? [String:Any]
        {
            if let rid = info["rid"] as? Int
            {
                let url : String = "\(config.IMAGE_DOMAIN)\(rid).png";
            
                if let url = URL(string: url) {
                    // 非同步載入圖片
                    DispatchQueue.global(qos: .userInitiated).async {
                        if let imageData = try? Data(contentsOf: url), let image = UIImage(data: imageData) {
                            DispatchQueue.main.async {
                                self.imgRole?.image = image;
                            }
                        }
                    }
                }
                
                getLevelAttr(rid: rid);
            }
            if let name = info["name"] as? String
            {
                tvName?.text = name;
            }
            if let description = info["description"] as? String
            {
                tvDescription?.text = description;
            }
            if let pid = info["pid"] as? Int
            {
                tvProfession?.text = String(pid);
            }
            if let gid = info["gid"] as? Int
            {
                tvGender?.text = String(gid);
            }
        }
    }
    
    func getLevelAttr(rid : Int)
    {
        var controller = APIController();
        controller.GetFgoRoleLevelAttr(input: ["rid":rid]){result in
            var resultData = result.getData();
            
            var hps : [Int] = [];
            var atks : [Int] = [];
            
            if let json = resultData as? [String:Any]
            {
                if let level_attr = json["level_attr"] as? [String:Any]
                {
                    if let data = level_attr["data"] as? [[String:Any]]
                    {
                        for item in data
                        {
                            if let hp = item["hp"] as? Int
                            {
                                hps.append(hp);
                            }
                            if let atk = item["atk"] as? Int
                            {
                                atks.append(atk);
                            }
                        }
                    }
                }
            }
            
            // 準備你的數據
            var entries1 : [ChartDataEntry] = [];
            var entries2 : [ChartDataEntry] = [];
            for (index,value) in hps.enumerated()
            {
                entries1.append(ChartDataEntry(x:Double(index),y:Double(value)));
            }
                                
            for (index,value) in atks.enumerated()
            {
                //entries2.append(ChartDataEntry(x:Double(index),y:Double(2000-value)));
                entries2.append(ChartDataEntry(x:Double(index),y:Double(value)));
            }
            
            
            let dataSet1 = LineChartDataSet(entries: entries1, label: "HP")
            dataSet1.colors = [.blue]
            dataSet1.valueTextColor = .black
            dataSet1.drawValuesEnabled = false // 預設不顯示數值
            dataSet1.lineWidth = 2;
            dataSet1.circleRadius = 0.1;
            
            let dataSet2 = LineChartDataSet(entries: entries2, label: "ATK")
            dataSet2.colors = [.red]
            dataSet2.valueTextColor = .black
            dataSet2.drawValuesEnabled = false // 預設不顯示數值
            dataSet2.lineWidth = 2;
            dataSet2.circleRadius = 0.1;
            
            let data = LineChartData(dataSets: [dataSet1, dataSet2])
            self.lineView.data = data

            self.lineView.delegate = self;
            // 其他圖表配置
            self.lineView.leftAxis.calculate(min: 0, max: 2000);
            self.lineView.rightAxis.calculate(min: 0, max: 2000);
            self.lineView.chartDescription.enabled = false
            self.lineView.isUserInteractionEnabled = true
            self.lineView.dragEnabled = true
            self.lineView.setScaleEnabled(true)
            self.lineView.animate(xAxisDuration: 1.0)
            self.lineView.invalidateIntrinsicContentSize();
        }
   }
}

extension RoleContentViewController: ChartViewDelegate
{
    // 實作 ChartViewDelegate 的方法
    func chartValueSelected(_ chartView: ChartViewBase, entry: ChartDataEntry, highlight: Highlight) {
        print("Value selected: \(entry)")
        self.tvLineNum.text = "Level : \(entry.x) = \(entry.y)"
    }

    func chartValueNothingSelected(_ chartView: ChartViewBase) {
        print("Nothing selected.")
        self.tvLineNum.text = "";
    }
}
