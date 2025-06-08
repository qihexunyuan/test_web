/**
 *  配置服务器路由（接口）
 */

const express = require('express')
const router = express.Router()
let user = require('./api/user')

// 教师相关接口
router.post('/user/list', user.list)       // 用户列表
router.post('/user/delete', user.delete)   // 删除用户
router.post('/user/add', user.add)         // 添加用户
router.post('/user/update', user.update)   // 更新用户

// 学生相关接口
router.post('/student/list', user.studentList)         // 学生列表
router.post('/student/delete', user.studentDelete)     // 删除学生
router.post('/student/add', user.studentAdd)           // 新增学生
router.post('/student/update', user.studentUpdate)     // 更新学生

module.exports = router
