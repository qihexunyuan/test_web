<template>
  <div>
    <!-- 新增用户按钮 -->
    <el-button type="primary" @click="handleAddUser">新增学生</el-button>
    <!-- 新增学生表单，默认隐藏 -->
    <el-dialog v-model="dialogVisible" title="新增学生">
      <el-form :model="userForm" label-width="120px">
        <el-form-item label="学生ID">
          <el-input v-model="userForm.id"></el-input>
        </el-form-item>
        <el-form-item label="姓名">
          <el-input v-model="userForm.name"></el-input>
        </el-form-item>
        <el-form-item label="数学">
          <el-input v-model.number="userForm.math"></el-input>
        </el-form-item>
        <el-form-item label="语文">
          <el-input v-model.number="userForm.chinese"></el-input>
        </el-form-item>
        <el-form-item label="英语">
          <el-input v-model.number="userForm.english"></el-input>
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="dialogVisible = false">取消</el-button>
          <el-button type="primary" @click="submitAddUser()">确定</el-button>
        </span>
      </template>
    </el-dialog>
    <!-- 更新学生表单，默认隐藏 -->
    <el-dialog v-model="updateDialogVisible" title="更新学生成绩">
      <el-form :model="updateForm" label-width="120px">
        <el-form-item label="姓名">
          <el-input v-model="updateForm.name"></el-input>
        </el-form-item>
        <el-form-item label="数学">
          <el-input v-model.number="updateForm.math"></el-input>
        </el-form-item>
        <el-form-item label="语文">
          <el-input v-model.number="updateForm.chinese"></el-input>
        </el-form-item>
        <el-form-item label="英语">
          <el-input v-model.number="updateForm.english"></el-input>
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="updateDialogVisible = false">取消</el-button>
          <el-button type="primary" @click="submitUpdateUser()">确定</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
  <div id="app">
    <!-- 标题 -->
    <h1>学生成绩管理系统</h1>
    <!-- 搜索框 -->
    <el-input
      v-model="searchKeyword"
      placeholder="请输入姓名关键字搜索"
      @input="handleSearch"
    ></el-input>
    <!-- 表格 -->
    <el-table :data="filteredStudents" style="width: 100%">
      <el-table-column prop="id" label="ID"></el-table-column>
      <el-table-column prop="name" label="姓名"></el-table-column>
      <el-table-column prop="math" label="数学"></el-table-column>
      <el-table-column prop="chinese" label="语文"></el-table-column>
      <el-table-column prop="english" label="英语"></el-table-column>
      <el-table-column label="操作">
        <template #default="scope">
          <el-button type="danger" @click="deleteStudent(scope.row.id)">删除</el-button>
          <el-button type="warning" @click="handleUpdateUser(scope.row)">更新</el-button>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import { ElMessage } from 'element-plus';
import { api } from './main.js';

// 控制对话框显示隐藏
const dialogVisible = ref(false);
// 控制更新对话框显示隐藏
const updateDialogVisible = ref(false);
// 新增学生表单数据，将成绩初始值设为数字 0
const userForm = ref({
  id: '',
  name: '',
  math: '',
  chinese: '',
  english: ''
});
// 更新学生表单数据
const updateForm = ref({
  id: '',
  name: '',
  math: '',
  chinese: '',
  english: ''
});

const students = ref([]); // 学生数据
// 搜索关键字
const searchKeyword = ref('');

// 分页相关参数
const pageIndex = ref(1);
const pageSize = ref(10);

// 过滤后的学生数据
const filteredStudents = computed(() => {
  if (!searchKeyword.value) {
    return students.value;
  }
  return students.value.filter(student =>
    student.name.includes(searchKeyword.value)
  );
});

// 搜索处理函数
const handleSearch = () => {
  // 搜索逻辑已在 computed 中实现，此处可留空
};

/**
 * 点击新增学生按钮触发，显示对话框
 */
const handleAddUser = () => {
  dialogVisible.value = true;
};

/**
 * 提交新增学生表单数据
 */
const submitAddUser = async () => {
  try {
    // 调用 api.addStudent 方法提交数据
    const res = await api.addStudent(userForm.value);
    ElMessage.success('学生添加成功');
    dialogVisible.value = false;
    // 清空表单数据
    userForm.value = {
      id: '',
      name: '',
      math: '',
      chinese: '',
      english: ''
    };
    // 重新获取学生列表
    await fetchStudentList();
  } catch (error) {
    ElMessage.error('学生添加失败，请重试');
  }
};

/**
 * 点击更新学生按钮触发，显示对话框
 * @param {Object} student 学生数据
 */
const handleUpdateUser = (student) => {
  updateForm.value = {
    id: student.id,
    name: student.name,
    math: student.math,
    chinese: student.chinese,
    english: student.english
  };
  updateDialogVisible.value = true;
};

/**
 * 提交更新学生成绩表单数据
 */
const submitUpdateUser = async () => {
  try {
    await api.updateStudent(updateForm.value); // 这样会带上 name
    ElMessage.success('学生成绩更新成功');
    updateDialogVisible.value = false;
    await fetchStudentList();
  } catch (error) {
    ElMessage.error('学生成绩更新失败，请重试');
  }
};

/**
 * 删除学生数据
 * @param {number} id 学生 ID
 */
const deleteStudent = async (id) => {
  try {
    // 调用 api.deleteStudent 方法删除数据
    await api.deleteStudent({ id });
    ElMessage.success('学生删除成功');
    // 重新获取学生列表
    await fetchStudentList();
  } catch (error) {
    ElMessage.error('学生删除失败，请重试');
  }
};

/**
 * 获取学生列表
 */
const fetchStudentList = async () => {
  try {
    const res = await api.getStudentList({ 
      pageIndex: pageIndex.value, 
      pageSize: pageSize.value, 
      searchKey: searchKeyword.value 
    });
    students.value = res.data.list;
  } catch (e) {
    ElMessage.error('获取学生列表失败');
  }
};

// 页面加载时获取数据库数据
onMounted(async () => {
  await fetchStudentList();
});
</script>

<style scoped>
h1 {
  text-align: center;
}
</style>

