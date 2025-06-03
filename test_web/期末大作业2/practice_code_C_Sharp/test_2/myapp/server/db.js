/**
 *  连接数据库
 */
const mysql = require('mysql2')
// 创建连接池
const db = mysql.createPool({
  host: '127.0.0.1',     // 改为 127.0.0.1
  user: 'root',          // 用户名，默认root
  password: '123456',  // 密码
  database: 'test'       // 需要连接的数据库
})
module.exports = db
