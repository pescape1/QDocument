import Vue from 'vue'
import Resource from 'vue-resource'
import ApprovalUsers from './components/ApprovalUsers.vue'

Vue.use(Resource)

new Vue({
  el: '#app',
    render: f => f(ApprovalUsers)
})