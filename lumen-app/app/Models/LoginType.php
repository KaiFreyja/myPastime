<?php
// 參考 Eloquest ORM 
// https://laravel.com/docs/8.x/eloquent 
namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class ThirdLoginType extends Model
{
	// connetcion db 
	public $connection = 'fgo_user';
	// 如果不指定預設會抓 funs 
	public $table = 'fgo_login_type';
	// pky 不指定預設會抓 ID  
	public $primaryKey = 'ltid';
	// pky 不是整數時
	//protected $keyType = 'string';
	// 關掉 Autoincrement 
	//public $incrementing = false;
	// 你可以指定預 新增修改的欄位 我建議未來都抓預設 不指定
	//const CREATED_AT = 'insertt';
	//const UPDATED_AT = 'modifyt';

	/**
	* The attributes that are mass assignable
	*
	* @var array
	*/
	public $fillable = [
	   'ltid',  
	   'name', 
	   'description',
	   'seq', 
	   'switch', 
	   'lock',
	   'created_at', 
	   'updated_at'
	];

	/**
	 * The attributes excluded from the model's JSON form.
	 *
	 * @var array
	 */
	public $hidden = [
				
	];




}
