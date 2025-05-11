//
//  MapPosController.swift
//  fristUiKit
//
//  Created by 尤凱 on 2025/5/6.
//

import UIKit;
import GoogleMaps;

class MapPosController : UIViewController
{
    override func viewDidLoad() {
        super.viewDidLoad();
        
        let camera = GMSCameraPosition.camera(withLatitude: 25.033671, longitude: 121.564427, zoom: 15.0)
        let mapView = GMSMapView.map(withFrame: self.view.bounds, camera: camera)
        self.view.addSubview(mapView)

        var controller = APIController();
        controller.GetFgoMapPos(input: [:]){result in
            if let json = result.getData() as? [String:Any]
            {
                if let map_pos = json["map_pos"] as? [String:Any]
                {
                    if let data = map_pos["data"] as? [[String:Any]]
                    {
                        for i in 0..<data.count
                        {
                            var oneData = data[i];
                            var lat : Double = 0;
                            var lng : Double = 0;
                            if let slat = oneData["lat"] as? String
                            {
                                if let nlat = Double(slat)
                                {
                                    lat = nlat;
                                }
                            }
                            if let slng = oneData["lng"] as? String
                            {
                                if let nlng = Double(slng)
                                {
                                    lng = nlng;
                                }
                            }
                            let marler = GMSMarker();
                            marler.position = CLLocationCoordinate2D(latitude: lat, longitude: lng);
                            marler.map = mapView;
                            
                        }
                    }
                }
            }
        }
        
        /*
        // 新增一個旗標
        let marker = GMSMarker()
        marker.position = CLLocationCoordinate2D(latitude: 25.033671, longitude: 121.564427)
        marker.title = "台北101"
        marker.snippet = "地標"
        marker.map = mapView*/
    }
}
