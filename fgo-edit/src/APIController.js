import axios from 'axios';

//let domain = "http://172.30.20.40/ClientApi/api/";
let domain = "http://192.168.0.170/api/";
function getRole(input,action)
{
    sendAPI("role",input,action);
}

function getOneRole(input,action)
{
    var rid = input.rid;
    sendAPI("role/" + rid,input,action);
}

function AddNewRole(input,action)
{
    postAPI("role",input,action);
}

function modifyRole(input,action)
{
    var rid = input.rid;
    putAPI("role/" + rid,input,action);
}

function getProfession(input,action)
{
    sendAPI("profession",input,action);
}

function getGender(input,action)
{
    sendAPI("gender",input,action);
}

function getRoleLevelAttr(input,action)
{
    sendAPI("level_attr",input,action);
}

function saveOneRoleLevelAttr(input,action)
{
    var laid = input.laid;
    putAPI("level_attr/"+laid,input,action);
}

function saveRoleLevelAttr(input,action)
{
    postAPI("level_attr_save",input,action);
}

function getMaterial(input,action)
{
  sendAPI("material",input,action);
}

function AddNewMaterial(input,action)
{
  postAPI("material",input,action);
}

function ModifyMaterial(input,action)
{
  var mid = input.mid;
  putAPI("material/"+mid,input,action);
}

function getProfessionAtkRate(input,action)
{
  sendAPI("profession_atk_rate",input,action);
}

function saveProfessionAtkRate(input,action)
{
  postAPI("profession_atk_rate_save",input,action);
}

function saveRoleImage(input,fileObject,action)
{
  postFile("upload_role_image",input,fileObject,action);
}

function getRoleResourceImage(input,action)
{
  sendAPI("role_resource",input,action);
}

function getFeature(input,action)
{
  sendAPI("feature",input,action);
}

function addNewFeature(input,action)
{
  postAPI("feature",input,action);
}

function mmodifyFeature(input,action)
{
  var fid = input.fid;
  putAPI("feature/"+fid,input,action);
}

function getMapPos(input,action)
{
  sendAPI("map_pos",input,action);
}

function addMapPos(input,action)
{
  postAPI("map_pos",input,action);
}

function delMapPos(input,action)
{
  var mpid = input.mpid;
  deleteAPI("map_pos/" + mpid,input,action);
}

function sendAPI(url,input,action)
{
    console.log("sendAPI : " + "\n" + "url : " + url);
    var uri = domain + url;   
      const queryString = objectToQueryString(input);
  // 如果查詢字串不為空，則將其附加到 URL 上
        if (queryString) {
            uri += "?" + queryString;
        }

    axios.get(uri)
        .then(function(response){
            console.log(response);
            action(response.data);
        }).catch(function(error)
        {
            console.log(error);
            action();
        }).finally(function(){

        });
}

function objectToQueryString(input) {
    
  const params = new URLSearchParams();
  for (const key in input) {
    if (Object.prototype.hasOwnProperty.call(input, key)) { // 使用安全的 hasOwnProperty 檢查
      params.append(key, input[key]);
    }
  }
  return params.toString();
}

function postAPI(url,input,action)
{
console.log("postAPI : " + "\n" + "url : " + url + "\n" + JSON.stringify(input));

    var uri = domain + url;
axios.post(uri, input)
  .then(function (response) {
    console.log(response);
    action(response.data);
  })
  .catch(function (error) {
    console.log(error);
    action();
  });
}

function putAPI(url,input,action)
{
console.log("putAPI : " + "\n" + "url : " + url + "\n" + JSON.stringify(input));

      var uri = domain + url;
axios.put(uri, input)
  .then(function (response) {
    console.log(response);
    action(response.data);
  })
  .catch(function (error) {
    console.log(error);
    action();
  });  
}

function deleteAPI(url,input,action)
{
console.log("deleteAPI : " + "\n" + "url : " + url + "\n" + JSON.stringify(input));

      var uri = domain + url;
axios.delete(uri, input)
  .then(function (response) {
    console.log(response);
    action(response.data);
  })
  .catch(function (error) {
    console.log(error);
    action();
  });  
}

function postFile(url, input, fileObject, action) {
  console.log("postFile : " + "\n" + "url : " + url + "\n" + JSON.stringify(input) + "\n" + "File Object : ", fileObject);

  var uri = domain + url;
  const formData = new FormData();

  // 將 input 中的其他資料添加到 FormData
  for (const key in input) {
    if (Object.prototype.hasOwnProperty.call(input, key)) {
      formData.append(key, input[key]);
    }
  }

  // 將檔案添加到 FormData
  if (fileObject) {
    formData.append('file', fileObject); // 直接將 File 物件放入 FormData
  }

  axios.post(uri, formData, {
    headers: {
      'Content-Type': 'multipart/form-data',
    },
  })
    .then(function (response) {
      console.log(response);
      action(response.data);
    })
    .catch(function (error) {
      console.log(error);
      action();
    });
}

function loadFile(filePath,action)
{
  if(filePath)
  {
    const reader = new FileReader();      
    reader.onload = (e) => {
      var fileObject = e.target.result;
      action(fileObject);
    };
    reader.readAsDataURL(filePath); // 開始讀取檔案為 Data URL
  }
  else
  {
    action();
  }
}

export {
    AddNewRole,
    modifyRole,
    getProfession,
    getGender,
    getRole,
    getOneRole,
    getRoleLevelAttr,
    saveRoleLevelAttr,
    saveOneRoleLevelAttr,
    saveRoleImage,
    getMaterial,
    AddNewMaterial,
    ModifyMaterial,
    saveProfessionAtkRate,
    getProfessionAtkRate,
    getFeature,
    addNewFeature,
    mmodifyFeature,
    getMapPos,
    addMapPos,
    delMapPos,
    getRoleResourceImage,
};