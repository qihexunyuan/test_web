import { createApp } from 'vue';
import ElementPlus from 'element-plus';
import 'element-plus/dist/index.css';
import 'element-plus/theme-chalk/dark/css-vars.css';
import zhCn from 'element-plus/dist/locale/zh-cn.mjs';
// 修改导入路径，确保大小写一致
import App from './app.vue'; 
import axios from 'axios';
import { ElMessage } from 'element-plus';

// 配置项
const service = axios.create({
  baseURL: 'http://localhost:3000/',    
  timeout: 5000,                            
});

// 请求拦截器
service.interceptors.request.use((config) => {
  return config;
}, err => {
  return Promise.reject(err);
});

// 响应拦截器
service.interceptors.response.use((res) => {
  if(res.status === 200 && res.data.code === 200) {
    return res.data;
  } else {
    ElMessage({message: `${res.data.code} - ${res.data.message}`, type: 'error'});
  }
}, function(err) {
  return Promise.reject(err);
});

/**
 * 封装get方法
 * @param url 请求的 URL 地址
 * @param params 请求携带的参数
 * @returns {Promise} 返回一个 Promise 对象
 */
export function get(url, params) {
  return new Promise((response, reject) => {
    service({url, method: 'get', params}).then(res => {
      response(res);
    }).catch(err => {
      reject(err);
    });
  });
}

/**
 * 封装 post 方法
 * @param url 请求的 URL 地址
 * @param data 请求携带的数据
 * @returns {Promise} 返回一个 Promise 对象
 */
export function post(url, data) {
  return new Promise((resolve, reject) => {
    service({ url, method: 'post', data }).then(res => {
      resolve(res);
    }).catch(err => {
      reject(err);
    });
  });
}

// 定义接口请求方法
export const api = {
  getStudentList: (data = {}) => post('/student/list', data),
  deleteStudent: (data = {}) => post('/student/delete', data),
  addStudent: (data = {}) => post('/student/add', data),
  updateStudent: (data = {}) => post('/student/update', data),

  getTeacherList: (data = {}) => post('/user/list', data),
  deleteTeacher: (data = {}) => post('/user/delete', data),
  addTeacher: (data = {}) => post('/user/add', data),
  updateTeacher: (data = {}) => post('/user/update', data),
};

// 创建 app 实例
const app = createApp(App);

app.use(ElementPlus, { locale: zhCn});

// 挂载应用到 DOM 元素
app.mount('#app');
