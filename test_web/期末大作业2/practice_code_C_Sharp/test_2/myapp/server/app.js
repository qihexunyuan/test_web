const express = require('express')
const app = express()
const cors = require('cors')
const bodyParser = require('body-parser')
const router = require('./router')

//配置解析，用于解析json和urlencoded格式的数据
app.use(bodyParser.json())
app.use(bodyParser.urlencoded({ extended: true }))
app.use(cors()) // 配置跨域
app.use(router) // 配置路由

app.get('/', (req, res) => {
  res.send('API服务已启动------修改------');
});

// 监听
app.listen(3000, () => {
  console.log('服务器启动成功...访问地址：http://localhost:3000');
})
