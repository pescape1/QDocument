import Vue from 'vue'
import Resource from 'vue-resource'

Vue.use(Resource)

Vue.component('approval-users', require('./components/ApprovalUsers.vue').default)

new Vue({
    el: '#app',
})