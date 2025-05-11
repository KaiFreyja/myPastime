/* eslint-disable camelcase */
// vue set 
//development npm run serve
//production npm run build
const baseUrl = process.env.NODE_ENV === 'production' ? '/' : './' ; 

const dev = {
  showLicense:true ,
  showSideNav:true,
  showApiTitle:false,
  usePHPMenu:true,
  useDotNetMenu:false
};

const config = { 
  restApiDriver: 'phpApiUrl', // default api driver 
  dotnetApiDriver: 'dotnetApiUrl',
  ebookApiDriver: 'ebookPhpApiUrl',  
  baseUrl,
  firebase : {
    apiKey: 'AIzaSyDTZ5mj0njYZHbIv0jdUTpvFLj8Pst3zuU',
    authDomain: 'sbgen5-testing.firebaseapp.com',
    projectId: 'sbgen5-testing',
    storageBucket: 'sbgen5-testing.appspot.com',
    messagingSenderId: '305389973704',
    appId: '1:305389973704:web:9af2f650e989687fadf5fd',
    measurementId: 'G-T6D9402NVH'
  },
  router : {
    max_histroy_length: 50,
  },
  isSkipPayment: false,
  externalConfigPath: '/data/2a23a8a2ec68042a', // example : 實際路徑 public/data/test，填入 /data/test
};

// restApiDriver 
const restApiDriver = {
  defaultApiUrl : 'http://172.30.20.28/ClientApi/api' ,
  dotnetApiUrl  : 'http://172.30.20.28:5080/api' ,
  phpApiUrl  : 'http://172.30.20.28/ClientApi/api' ,
  // ebookPhpApiUrl  : 'http://172.30.10.126/AdminApi/api' ,
  ebookPhpApiUrl  : 'http://172.30.20.140/AdminApi/api' , // 正式機
};

const loginType = {
  general: 'general',
  facebook: 'facebook',
  google: 'google',
  appleid: 'appleid',
  email: 'email',
  phone: 'phone',
};

const userType = {
  guest: 'guest',
  member: 'member',
  admin: 'admin',
  sub_member: 'sub_member',
  teacher: 'teacher',
  student: 'student',
  cram_teacher: 'cram_teacher',
  cram_student: 'cram_student',
};

const essayMode = {
  further: 'further',
  news: 'news',
};

const productGenre = {
  lesson: '1', // 經典課程
  exam: '2', // 百萬題庫
  lesson_exam: '3', // 經典課程+百萬題庫
};

/*
* Yves modify 1. SBDW5-2000，將單純字串改成物件使用
*/
const defaultYearCode = {
  num: '112',
};

//年級範圍
const gradeRange = {
  elementary: '12',
  junior: '18',
  senior: '24',
};

const ctbcbank = {
  payOff: {                                       //一般交易    
    merID: '78046',                               //特店編號    
    MerchantID: '8220300000057',                  //特店代號
    TerminalID: '90300058',                       //終端機代號
    Key: 'Hq3nN1b4BIeo8nAV0oTLxAGs',              //壓碼字串
    txType: '0',                                  //交易方式, 一般交易：0, 分期交易：1
    Option: '1',                                  //一般交易請填「1」, 分期交易請填一到兩碼的分期期數(3、6、9、12、24)
    AutoCap: '1',                                 //是否自動請款
    Customize: '0',                               //刷卡頁顯示特定語系
    debug: '1',                                   //預設(進行交易時)請填 0，偵錯時請填 1
    MerchantName: '口袋學校',                     //特店名稱
    action: 'http://172.30.20.22/ctbcbank/index.php',    //付款網址
    errorReturnUrl: 'http://172.30.20.22/ShoppingCart',          //錯誤跳回網址
    authResURL:'http://172.30.20.22/ctbcbank/pay_off_result.php', //付款完結果回傳網址
  },
  installment: {                                  //分期交易
    merID: '78047',                               //特店編號    
    MerchantID: '8220310000037',                  //特店代號
    TerminalID: '91300038',                       //終端機代號
    Key: 'tILwAgP5zUJfdjEQBrclN5aA',              //壓碼字串
    txType: '1',                                  //交易方式, 一般交易：0, 分期交易：1
    Option: '3',                                  //一般交易請填「1」, 分期交易請填一到兩碼的分期期數(3、6、9、12、24)
    AutoCap: '1',                                 //是否自動請款
    Customize: '0',                               //刷卡頁顯示特定語系
    debug: '1',                                   //預設(進行交易時)請填 0，偵錯時請填 1
    MerchantName: '口袋學校',                     //特店名稱
    action: 'http://172.30.20.22/ctbcbank/index.php',    //付款網址
    errorReturnUrl: 'http://172.30.20.22/ShoppingCart',          //錯誤跳回網址
    authResURL:'http://172.30.20.22/ctbcbank/installment_result.php', //付款完結果回傳網址
  }
};

const payStatus = {
  paid: 'paid',
  unpaid: 'unpaid',
  refunded: 'refunded',
  cancel: 'cancel',
  fail: 'fail',
  paying: 'paying',
  waitpay: 'waitpay',
  renew: 'renew',
};

const paymentCompany = {
  default: 'ecpay',
  ecpay: 'ecpay',
  ctbcbank: 'ctbcbank',
};

const paymentType = {
  default: 'credit_card',
  credit_card: 'credit_card',
  credit_card_installment: 'credit_card_installment',
  atm: 'atm',
  supermarket: 'supermarket',
  point: 'point',
};

const orderItemPriceType = {
  original: 'original',
  discount: 'discount',
};

const modulesList = {
  promo: true,
  routerViewPermission: false, // 畫面權限檢查
  checkSubDomain: false, // 檢查子網域，切換一般版、Lite 版
  subDomainList:[
    {
      'key' : 'general', // 一般版
      'domain' : '172.30.20.22',
    },
    {
      'key' : 'lite', // Lite 版
      'domain' : '172.30.20.22:8081',
    },
  ], // 子網域清單
  checkIpAllowList: true, // 檢查 IP 白名單模組開關
  voucher: true, // 序號模組
  learningMap: true, // 學習地圖模組
  productGroupList: true, // 產品包套模組
  checkExperience: true, // Lite 版體驗QRcode
  questionListLite: false, // 師生解惑模組
  renewOrder: false, // 訂單續購模組
  curriculumPlatform: true, // 新版課程/題庫/電子書
};

//因學制單獨開放使用新題庫
const examNewEducationSystem = {
  code_e:false, //國小
  code_j:true, //國中
  code_s:false, //高中
}

const choosePayment = {
  credit: 'Credit', //信用卡及銀聯卡(需申請開通)
  webatm: 'WebATM', //網路ATM
  atm: 'ATM', //自動櫃員機
  cvs: 'CVS', //超商代碼
  barcode: 'BARCODE', //超商條碼
  applepay: 'ApplePay', //Apple Pay(僅支援手機支付)
  all: 'ALL', //不指定付款方式，由綠界顯示付款方式選擇頁面。
};

const obtainType = {
  buy_product: 'buy_product', //購買商品
  exchange_voucher: 'exchange_voucher', //序號兌換
  admin_creat: 'admin_creat', //後台建立
};

const productShowPage = {
  official_website: 'official_website', //官網
  shopline: 'shopline', //一頁式網站
};

const productActivationMethod = {
  same_time: 'same_time', //同時啟用
  sequence: 'sequence', //依順序啟用
};

const productMode = {
  single: 'single', //獨立商品
  group: 'group', //包套商品
};

const reviewExamType = {
  type: 'json',
};

const downloadExamPaperConfig = {
  'domain' : 'uat.podschool.com.tw',
  'protocol' : 'http://',
  'path' : '/DownloadExamPaper',
  'pathCombine' : '/DownloadExamPaperCombine',
  'aesKey': 'nhuiasxinyasxinydongbidongbinhui',
  'aesIv': 'tgvfrjmnbyhpoilk',
};

const externalWeb = {
  client : 'https://st-sbdw5.olttn.com/',
};

export { config, dev , restApiDriver, loginType, essayMode, userType, defaultYearCode, productGenre, ctbcbank, payStatus, paymentCompany, paymentType, orderItemPriceType, modulesList, choosePayment, obtainType , gradeRange, productShowPage, productActivationMethod, productMode, reviewExamType, examNewEducationSystem ,downloadExamPaperConfig,externalWeb};
