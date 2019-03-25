<template>
    <div>
        <table class="table table-striped table-bordered table-sm">
            <tr v-if="!users.length">
                <td colspan="2">No users.</td>
            </tr>
            <tr v-for="(user, index) in users">
                <td><span>{{ user.fullName }}</span></td>
                <td><span>{{ user.jobTitle }}</span></td>
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
        mounted: function () {
            var vm = this;
            $(this.jlist).on('change', vm, function (e) {
                e.data.asigna($(this).val());
            });
            this.asigna($(this.jlist).val());
        },
        methods: {
            asigna: function (jobList) {
                this.$http
                    .get('/api/GetApprovalUsers', { params: { jobList: JSON.stringify(jobList) }})
                    .then((res) => {
                        this.users = [];
                        for (var i = 0; i < res.body.length; i++) {
                            this.users.push(res.body[i]);
                        }
                    })
                    .catch((ex) => console.log(ex))
            }
        }
    }
</script>