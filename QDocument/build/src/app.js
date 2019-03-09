import Vue from 'vue'
import Resource from 'vue-resource'
//import ApprovalUsers from './components/ApprovalUsers.vue'

Vue.use(Resource)

Vue.component('approval-users1', require('./components/ApprovalUsers.vue').default);

new Vue({
  el: '#app',
    //render: f => f(ApprovalUsers, { props: { jlist: ' jlist prop' } })
})