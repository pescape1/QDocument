<template>
    <div>
        <table>
            <tr v-if="!users.length">
                <td colspan="1">No users.</td>
            </tr>
            <tr v-for="(user, index) in users">
                <td><span>{{ user.text }}</span></td>
            </tr>
        </table>
    </div>
</template>

<script>
    export default {
        props: {
            jlist: {
                type: String,
                required: true
            }
        },
        //name: 'approval-users',
        data() {
            return {
                users: []
            }
        },
        // Send a request to /api/hello
        created: function () {
            var vm = this;
            $(this.jlist)
                .on('change', vm, function (e) {
                    e.data.asigna($(this).val())
                    //Vue.set(this.$data, 'users', $(this.jlist.val()));
                    //this.users = $(this.jlist.val()).slice(0);
                    //console.log(this.users);
                })
        },
        /*created: function() {
                console.log(this.jlist);
                //Vue.set(this.$data, 'users', $(jlist.val()));
                this.$http
                    .get('/api/GetApprovalUsers') //, { params: { jobList } })
                    .then((res) => {
                        for (var i = 0; i < res.body.length; i++) {
                            this.users.push(res.body[i]);
                        }
                        console.log(res.body);
                    })
                    .catch((ex) => console.log(ex))
            
        },*/
        methods: {
            asigna: function (value) {
                //this.users = value.slice(0);
                this.users = [];
                for (var i = 0; i < value.length; i++) {
                    this.users.push({
                        'text': value[i]
                    })
                }
                console.log(this.users);
            }
        }
    }
</script>