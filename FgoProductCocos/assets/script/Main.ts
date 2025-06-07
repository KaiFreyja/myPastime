import { _decorator, Component, Node } from 'cc';
import { ViewController } from './ViewController/ViewController';
import { RoleListViewController } from './ViewController/RoleListViewController';
import { RoleContentViewController } from './ViewController/RoleContentViewController';
const { ccclass, property } = _decorator;

@ccclass('Main')
export class Main extends Component {
    async start() {   
        var view = await ViewController.GetViewController(RoleListViewController);
        var view2 = await ViewController.GetViewController(RoleContentViewController);
        view.show();
        view2.show();     
    }

    update(deltaTime: number) {
        
    }
}


