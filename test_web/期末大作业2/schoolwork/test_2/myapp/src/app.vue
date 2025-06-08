<template>
  <div>
    <!-- 新增学生按钮 -->
    <el-button type="primary" @click="handleAddUser">新增学生</el-button>
    <!-- 教师管理按钮 -->
    <el-button type="primary" @click="handleTeacherManagement">教师管理</el-button>
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
    <!-- 新增教师表单，默认隐藏 -->
    <el-dialog v-model="teacherDialogVisible" title="新增教师">
      <el-form :model="teacherForm" label-width="120px">
        <el-form-item label="教师姓名">
          <el-input v-model="teacherForm.name"></el-input>
        </el-form-item>
        <el-form-item label="教授科目">
          <el-input v-model="teacherForm.subject"></el-input>
        </el-form-item>
        <el-form-item label="教授班级">
          <el-input v-model="teacherForm.class"></el-input>
        </el-form-item>
        <el-form-item label="科目总学时">
          <el-input v-model.number="teacherForm.totalHours"></el-input>
        </el-form-item>
        <el-form-item label="班级人数">
          <el-input v-model.number="teacherForm.classSize"></el-input>
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="teacherDialogVisible = false">取消</el-button>
          <el-button type="primary" @click="submitAddTeacher()">确定</el-button>
        </span>
      </template>
    </el-dialog>
    <!-- 教师列表表格 -->
    <div v-if="showTeacherManagement">
      <div id="app">
        <h1>教师管理系统</h1>
        <!-- 搜索框 -->
        <el-input
          v-model="teacherSearchKeyword"
          placeholder="请输入教师姓名关键字搜索"
          @input="handleTeacherSearch"
        ></el-input>
        <!-- 表格 -->
        <el-table :data="filteredTeachers" style="width: 100%">
          <el-table-column prop="id" label="ID"></el-table-column>
          <el-table-column prop="name" label="教师姓名"></el-table-column>
          <el-table-column prop="subject" label="教授科目"></el-table-column>
          <el-table-column prop="class" label="教授班级"></el-table-column>
          <el-table-column prop="totalHours" label="科目总学时"></el-table-column>
          <el-table-column prop="classSize" label="班级人数"></el-table-column>
          <el-table-column label="操作">
            <template #default="scope">
              <el-button type="danger" @click="deleteTeacher(scope.row.id)">删除</el-button>
              <el-button type="warning" @click="handleUpdateTeacher(scope.row)">更新</el-button>
            </template>
          </el-table-column>
        </el-table>
        <!-- 新增教师按钮 -->
        <el-button type="primary" @click="handleAddTeacher">新增教师</el-button>
      </div>
    </div>
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

// 控制教师管理界面显示隐藏
const showTeacherManagement = ref(false);
// 控制新增教师对话框显示隐藏
const teacherDialogVisible = ref(false);
// 控制更新教师对话框显示隐藏
const updateTeacherDialogVisible = ref(false);
// 新增教师表单数据
const teacherForm = ref({
  id: '',
  name: '',
  subject: '',
  class: '',
  totalHours: '',
  classSize: ''
});
// 更新教师表单数据
const updateTeacherForm = ref({
  id: '',
  name: '',
  subject: '',
  class: '',
  totalHours: '',
  classSize: ''
});

const teachers = ref([]); // 教师数据
// 教师搜索关键字
const teacherSearchKeyword = ref('');

// 分页相关参数
const teacherPageIndex = ref(1);
const teacherPageSize = ref(10);

// 过滤后的教师数据
const filteredTeachers = computed(() => {
  if (!teacherSearchKeyword.value) {
    return teachers.value;
  }
  return teachers.value.filter(teacher =>
    teacher.name.includes(teacherSearchKeyword.value)
  );
});

// 教师搜索处理函数
const handleTeacherSearch = () => {
  // 搜索逻辑已在 computed 中实现，此处可留空
};

/**
 * 点击教师管理按钮触发，显示教师管理界面
 */
const handleTeacherManagement = async () => {
  showTeacherManagement.value = true;
  await fetchTeacherList();
};

/**
 * 点击新增教师按钮触发，显示对话框
 */
const handleAddTeacher = () => {
  teacherDialogVisible.value = true;
};

/**
 * 提交新增教师表单数据
 */
const submitAddTeacher = async () => {
  try {
    // 调用 api.addTeacher 方法提交数据
    const res = await api.addTeacher(teacherForm.value);
    ElMessage.success('教师添加成功');
    teacherDialogVisible.value = false;
    // 清空表单数据
    teacherForm.value = {
      id: '',
      name: '',
      subject: '',
      class: '',
      totalHours: '',
      classSize: ''
    };
    // 重新获取教师列表
    await fetchTeacherList();
  } catch (error) {
    ElMessage.error('教师添加失败，请重试');
  }
};

/**
 * 点击更新教师按钮触发，显示对话框
 * @param {Object} teacher 教师数据
 */
const handleUpdateTeacher = (teacher) => {
  updateTeacherForm.value = {
    id: teacher.id,
    name: teacher.name,
    subject: teacher.subject,
    class: teacher.class,
    totalHours: teacher.totalHours,
    classSize: teacher.classSize
  };
  updateTeacherDialogVisible.value = true;
};

/**
 * 提交更新教师信息表单数据
 */
const submitUpdateTeacher = async () => {
  try {
    await api.updateTeacher(updateTeacherForm.value);
    ElMessage.success('教师信息更新成功');
    updateTeacherDialogVisible.value = false;
    await fetchTeacherList();
  } catch (error) {
    ElMessage.error('教师信息更新失败，请重试');
  }
};

/**
 * 删除教师数据
 * @param {number} id 教师 ID
 */
const deleteTeacher = async (id) => {
  try {
    // 调用 api.deleteTeacher 方法删除数据
    await api.deleteTeacher({ id });
    ElMessage.success('教师删除成功');
    // 重新获取教师列表
    await fetchTeacherList();
  } catch (error) {
    ElMessage.error('教师删除失败，请重试');
  }
};

/**
 * 获取教师列表
 */
const fetchTeacherList = async () => {
  try {
    const res = await api.getTeacherList({ 
      pageIndex: teacherPageIndex.value, 
      pageSize: teacherPageSize.value, 
      searchKey: teacherSearchKeyword.value 
    });
    teachers.value = res.data.list;
  } catch (e) {
    ElMessage.error('获取教师列表失败');
  }
};
</script>

<style scoped>
h1 {
  text-align: center;
}
</style>

