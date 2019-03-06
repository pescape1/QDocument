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
        props: ['jobList'],
        //name: 'hello',
        data() {
            return {
                users: []
            }
        },
        // Send a request to /api/hello
        created() {
            this.$http
                .get('/api/GetApprovalUsers', { params: { jobList } })
                .then((res) => {
                    for (var i = 0; i < res.body.length; i++) {
                        this.users.push(res.body[i]);
                    }
                    console.log(this.users);
                })
                .catch((ex) => console.log(ex))
        }
    }
</script>