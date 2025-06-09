const db = require('../db')

// 用户列表
exports.list = (req, res) => {
  const { pageIndex, pageSize, searchKey } = req.body;
  db.query(`select count(*) as total from teacher where name like '%${searchKey}%'`, (err, data1) => {
    if(err) return res.send('错误：' + err.message);
    const total = data1[0].total;
    const offset = (pageIndex - 1) * pageSize;
    const sql = `select * from teacher where name like '%${searchKey}%' limit ${pageSize} offset ${offset}`
    db.query(sql, (err, data2) => {
      if(err) return res.send('错误：' + err.message);
      res.send({
        code: 200,
        message: 'success',
        data: {
          list: data2,
          total: Number(total),
          current: Number(pageIndex)
        }
      })
    })
  })
}

// 获取学生列表
exports.studentList = (req, res) => {
  const { pageIndex, pageSize, searchKey } = req.body;
  db.query(`select count(*) as total from student where name like '%${searchKey}%'`, (err, data1) => {
    if (err) return res.send('错误：' + err.message);
    const total = data1[0].total;
    const offset = (pageIndex - 1) * pageSize;
    const sql = `select * from student where name like '%${searchKey}%' limit ${pageSize} offset ${offset}`;
    db.query(sql, (err, data2) => {
      if (err) return res.send('错误：' + err.message);
      res.send({
        code: 200,
        message: 'success',
        data: {
          list: data2,
          total: Number(total),
          current: Number(pageIndex)
        }
      });
    });
  });
};

// 删除教师
exports.delete = (req, res) => {
  const { id } = req.body;
  db.query('DELETE FROM teacher WHERE id=?', [id], (err, result) => {
    if (err) return res.send('错误：' + err.message);
    res.send({ code: 200, message: '删除成功' });
  });
};

// 添加教师
exports.add = (req, res) => {
  const { id, name, subject, class: className, totalHours, classSize } = req.body;
  db.query('INSERT INTO teacher (id, name, subject, class, totalHours, classSize) VALUES (?, ?, ?, ?, ?, ?)', [id, name, subject, className, totalHours, classSize], (err, result) => {
    if (err) return res.send({ code: 500, message: '添加失败: ' + err.message });
    res.send({ code: 200, message: '添加成功' });
  });
};

// 更新教师
exports.update = (req, res) => {
  const { id, name, subject, class: className, totalHours, classSize } = req.body;
  db.query(
    'UPDATE teacher SET name=?, subject=?, class=?, totalHours=?, classSize=? WHERE id=?',
    [name, subject, className, totalHours, classSize, id],
    (err, result) => {
      if (err) return res.send({ code: 500, message: '更新失败: ' + err.message });
      res.send({ code: 200, message: '更新成功' });
    }
  );
};

// 删除学生
exports.studentDelete = (req, res) => {
  const { id } = req.body;
  db.query('DELETE FROM student WHERE id=?', [id], (err, result) => {
    if (err) return res.send('错误：' + err.message);
    res.send({ code: 200, message: '删除成功' });
  });
};

// 添加学生
exports.studentAdd = (req, res) => {
  const { id, name, math, chinese, english } = req.body;
  db.query(
    'INSERT INTO student (id, name, math, chinese, english) VALUES (?, ?, ?, ?, ?)',
    [id, name, math, chinese, english],
    (err, result) => {
      if (err) return res.send('错误：' + err.message);
      res.send({ code: 200, message: '添加成功' });
    }
  );
};

// 更新学生
exports.studentUpdate = (req, res) => {
  const { id, name, math, chinese, english } = req.body;
  db.query('UPDATE student SET name=?, math=?, chinese=?, english=? WHERE id=?', [name, math, chinese, english, id], (err, result) => {
    if (err) return res.send('错误：' + err.message);
    res.send({ code: 200, message: '更新成功' });
  });
};
