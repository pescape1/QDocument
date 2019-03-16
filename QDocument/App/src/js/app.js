import Vue from 'vue'
import Resource from 'vue-resource'

import '../scss/styles.scss'

Vue.use(Resource)

Vue.component('approval-users', require('../components/ApprovalUsers.vue').default)

new Vue({
    el: '#app',
})