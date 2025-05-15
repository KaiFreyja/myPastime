/* eslint-disable camelcase */
// vue set 
//development npm run serve
//production npm run build
const baseUrl = process.env.NODE_ENV === 'production' ? '/' : './' ; 

const config = { 
  APIDOMAIN : "http://192.168.0.170/api/",
};


export { config,};
