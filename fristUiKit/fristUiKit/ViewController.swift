//
//  ViewController.swift
//  fristUiKit
//
//  Created by 尤凱 on 2025/4/16.
//

import UIKit

class ViewController: UIViewController {

    @IBOutlet weak var myLabek: UILabel!
    @IBAction func Clickto(_ sender: UIButton) {
        
        let controller = APIController();
        controller.GetRole(input: [:]) {result in
            let json = result.getData();
            self.myLabek.text = "\(json)";
        }
        
    }
    
    
    @IBAction func GetGender(_ sender: UIButton) {
        let controller = APIController();
        controller.GetGender(input : [:]){result in
            let json = result.getData();
            self.myLabek.text = "\(json)";
        }
    }
    
    @IBAction func GetProfession(_ sender: UIButton) {
        
        let controller = APIController();
        controller.GetProfession(input:[:]){result in
            let json = result.getData();
            self.myLabek.text = "\(json)";
        }
    }
    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view.
    }

}

