//
//  RoleContentViewController.swift
//  fristUiKit
//
//  Created by 尤凱 on 2025/4/21.
//

import UIKit
import DGCharts
import SwiftyJSON;

class RoleContentViewController: UIViewController, UICollectionViewDelegate, UICollectionViewDataSource, UICollectionViewDelegateFlowLayout {
    
    @IBOutlet weak var imgRole: UIImageView!
    @IBOutlet weak var tvName: UILabel!
    @IBOutlet weak var tvDescription: UILabel!
    @IBOutlet weak var tvGender: UILabel!
    @IBOutlet weak var tvProfession: UILabel!
    
    @IBOutlet weak var lineView: LineChartView!
    
    @IBOutlet weak var tvLineNum: UILabel!
    //---------------------role resource-----------------------------
    @IBOutlet weak var collectionView: UICollectionView!
    var displayedImages: [String] = [];
    var timer : Timer?
    
    var roleInfo : [String:Any]?
    
    override func viewDidLoad() {
        super.viewDidLoad();
        
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

                getRoleResource(rid: rid);
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
    
    func getRoleResource(rid : Int)
    {
        // 設定 layout，而不是重新 new 一個 collectionView
        let layout = UICollectionViewFlowLayout()
        layout.scrollDirection = .horizontal
        layout.minimumLineSpacing = 0
        collectionView.setCollectionViewLayout(layout, animated: false)
        collectionView.isPagingEnabled = true
        collectionView.delegate = self
        collectionView.dataSource = self
        collectionView.showsHorizontalScrollIndicator = false
        collectionView.register(UICollectionViewCell.self, forCellWithReuseIdentifier: "cell")

        collectionView.translatesAutoresizingMaskIntoConstraints = false
        NSLayoutConstraint.activate([
            //collectionView.topAnchor.constraint(equalTo: scrollView.topAnchor),
            //collectionView.leadingAnchor.constraint(equalTo: scrollView.leadingAnchor),
            //collectionView.trailingAnchor.constraint(equalTo: scrollView.trailingAnchor),
            collectionView.heightAnchor.constraint(equalToConstant: 500) // 可依照實際情況調整
        ])
        
        var controller = APIController();
        var input = ["rid":rid];
        controller.GetRoleResource(input: input){result in
            var json = JSON(result.getData());
            var data = json["role_resource"]["data"];
            
            for (key, item) in data
            {
                self.displayedImages.append(item["url"].stringValue);
            }
            self.displayedImages.append(self.displayedImages[0]);
            self.collectionView.reloadData();
            
            if self.displayedImages.count > 1 {
                self.collectionView.scrollToItem(at: IndexPath(item: 1, section: 0), at: .centeredHorizontally, animated: false)
            }

            self.startAutoScroll();
        }
    }
    
    /*=====================================================================*/
    
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return displayedImages.count
    }

    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "cell", for: indexPath)

        // 清除舊的 subviews
        cell.contentView.subviews.forEach { $0.removeFromSuperview() }

        let imageView = UIImageView()
        imageView.contentMode = .scaleAspectFit
        imageView.clipsToBounds = true
        imageView.translatesAutoresizingMaskIntoConstraints = false
        cell.contentView.addSubview(imageView)

        // 設定 Auto Layout
        NSLayoutConstraint.activate([
            imageView.topAnchor.constraint(equalTo: cell.contentView.topAnchor),
            imageView.bottomAnchor.constraint(equalTo: cell.contentView.bottomAnchor),
            imageView.leadingAnchor.constraint(equalTo: cell.contentView.leadingAnchor),
            imageView.trailingAnchor.constraint(equalTo: cell.contentView.trailingAnchor)
        ])

        let imageName = displayedImages[indexPath.item]
        print("imageName : " + imageName)

        if let url = URL(string: imageName) {
            DispatchQueue.global(qos: .userInitiated).async {
                if let data = try? Data(contentsOf: url),
                   let image = UIImage(data: data) {
                    DispatchQueue.main.async {
                        imageView.image = image
                    }
                }
            }
        }
        return cell;
    }

    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
        return collectionView.frame.size
    }

    func scrollViewDidEndDecelerating(_ scrollView: UIScrollView) {
        adjustIfNeeded()
    }

    func scrollViewDidEndScrollingAnimation(_ scrollView: UIScrollView) {
        adjustIfNeeded()
    }

    func adjustIfNeeded() {
        let page = Int(collectionView.contentOffset.x / collectionView.frame.size.width)
        if page == 0 {
            collectionView.scrollToItem(at: IndexPath(item: displayedImages.count - 2, section: 0), at: .centeredHorizontally, animated: false)
        } else if page == displayedImages.count - 1 {
            collectionView.scrollToItem(at: IndexPath(item: 1, section: 0), at: .centeredHorizontally, animated: false)
        }
    }

    func startAutoScroll() {
        timer = Timer.scheduledTimer(withTimeInterval: 3.0, repeats: true) { _ in
            let current = Int(self.collectionView.contentOffset.x / self.collectionView.frame.size.width)
            let next = current + 1
            self.collectionView.scrollToItem(at: IndexPath(item: next, section: 0), at: .centeredHorizontally, animated: true)
        }
    }

    deinit {
        timer?.invalidate()
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
