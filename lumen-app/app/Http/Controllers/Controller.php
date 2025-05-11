<?php

namespace App\Http\Controllers;

use Laravel\Lumen\Routing\Controller as BaseController;

class Controller extends BaseController
{
    //


    protected function getApiName($uri,$postOnePkyIsString = ''){


		//if(isset($uri)){
		$arr = explode("/",$uri);

		if($postOnePkyIsString == true){
			$anum = count($arr) - 2;
		}
		else{
			$anum = count($arr) - 1;
			if(preg_match("/^([0-9]+)$/",$arr[$anum])){
				//return $arr[$anum];
				$anum = count($arr) - 2;

			}
		}
		//echo $anum ; exit ;
		if(isset($arr[$anum])){
			//return str_replace("/api/","",$uri) ;
			return $arr[$anum] ; 
		}
		echo "Please input uri to this method!!"; exit ; 

	}

    protected function getApiParams($request){

		$arr = array();

		if($request->input("pageSize")){
			$arr["paginate"] = $request->input("pageSize") ;
		}
		else{
			$arr["paginate"] = $request->input("paginate") ;
		}

		if($request->input("pageFilter")){
			$arr["filter"] = $request->input("pageFilter") ;
		}
		else{
			$arr["filter"] = $request->input("filter") ;
		}

		if($request->input("pageSort")){
			$arr["sort"] = $request->input("pageSort") ;
		}
		else{
			$arr["sort"] = $request->input("sort") ;
		}

		if($request->input("pageSortBy")){
			$arr["sortby"] = $request->input("pageSortBy") ;
		}
		else{
			$arr["sortby"] = $request->input("sortby") ;
		}


		if($request->input("pageGroup")){
			$arr["group"] = $request->input("pageGroup") ;
		}
		else{
			$arr["group"] = $request->input("group") ;
		}



		if($request->input("pageGroupBy")){
			$arr["groupby"] = $request->input("pageGroupBy") ;
		}
		else{
			$arr["groupby"] = $request->input("groupby") ;
		}

		return $arr ; 

	}
    
	protected function getApiSearchs($request){

		$arr = array();
		$rarr = array();
		$parr = array();
		//$rarr = $request->all() ;
		if(isset($request->pageSearch))
		{
			$parr = json_decode($request->pageSearch , true);
		}
		if(is_array($request->all())){
			foreach($request->all() as $rk => $rv){
				if(
					$rk != 'uri' &&
					$rk != 'restApiDriver' &&
					$rk != 'pageNumber' &&
					$rk != 'pageSize' &&
					$rk != 'pageSort' &&
					$rk != 'pageSortBy' &&
					$rk != 'pageFilter' &&
					$rk != 'pageSearch' &&
					$rk != 'pageGroup' &&
					$rk != 'pageGroupBy' &&
					$rk != 'pageMode' &&
					$rk != 'token' &&
					$rk != 'module' &&
					$rk != 'fun' &&
					$rk != 'act'
				){
					$rarr[$rk] = $rv ; 
				}

			}
		}
		/**
		 * pageSearch 如果碰到 直接從 Request 下 GET 進來 則 Request 會覆蓋 pageSearch 的內容
		 * 例如 pageSearch : {"uid":"5","account":"admin","name":"admin","rid":1,"roleName":"","email":"admin@gmail","switch":true}
		 * $request->name = 'guest'
		 * Mearge 後 $request->name 會覆蓋 pageSearch 的內容
		 */
		$arr = array_merge($parr,$rarr);
		/*
		echo "Request : \n";
		print_r($request->all());
		echo "Request Arr : \n";
		print_r($rarr);
		echo "PageSearch Arr : \n";
		print_r($parr);
		echo "Merge Arr : \n";
		print_r($arr);
		*/
		return $arr ; 

	}

		protected function fillInTheValueFromBoolean ($res) {
		
		// Work Around Code. Because the Postman can't be filled in the Boolean value, the value need to be filled in.
		if(is_array($res)) {
			$length = count($res);
			
			for ($index = 0; $length > $index; $index++) {
				if ($res[$index] === 0 || $res[$index] === '0' || $res[$index] === false || $res[$index] === "false") {
					$res[$index] = 0;
				}else if ($res[$index] === 1 || $res[$index] === '1' || $res[$index] === true|| $res[$index] === "true") {
					$res[$index] = 1;

				} else {
					throw new \InvalidArgumentException('錯誤的資料型別，該欄位只規定 1 0 布林代數 true false:'.$res[$index]);
				}
			}
		} else {
			if ($res === 0 || $res === '0' || $res === false|| $res === "false") {
				$res = 0;
			} else if ($res === 1 || $res === '1' || $res === true|| $res === "true") {
				$res = 1;
			} else {
				throw new \InvalidArgumentException('錯誤的資料型別，該欄位只規定 1 0 布林代數 true false:'.$res);
			}			
		}

		return $res;

	}

		public function postInsertFieldQuery($request,$query){
		$inputArr = array();
		$searchArr = $this->getApiSearchs($request);
		if(isset($searchArr) && is_array($searchArr)){
			foreach($searchArr as $sk => $sv){
				$i=0;
				if($this->isFillable($query,$sk)){
					//$updateArr[$sk] = $sv ; 
					if(isset($request[$sk])){
						//echo $sk." => ".$sv. "\n" ; 
						$query->$sk = $request[$sk] ; 
					}
					$i++;
				}
			}
		}
		return $query ; 
	}

		public function isFillable($model,$field){
		$fillable = $model->fillable;


		if(isset($fillable[0])){
			foreach($fillable as $fk => $fv){
				if($field == $fv){
					return true ;
				}
			}
		}
		return false ; 
	}

		public function getPutUpdateArray($request,$model){
		$updateArr = array();
		$searchArr = $this->getApiSearchs($request);
		//print_r($searchArr); exit ;
		if(isset($searchArr) && is_array($searchArr)){
			foreach($searchArr as $sk => $sv){
				$i=0;
				if($this->isFillable($model,$sk)){
					$updateArr[$sk] = $sv ; 
					$i++;
				}
			}
		}
		//print_r($updateArr); exit ;
		return $updateArr ; 
	}
}
