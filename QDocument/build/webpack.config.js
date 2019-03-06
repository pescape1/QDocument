'use strict'
const { VueLoaderPlugin } = require('vue-loader');
const path = require('path');
module.exports = {
  mode: 'development',
  entry: [
    path.resolve(__dirname,'src/app.js')
  ],
  output: {
        path: path.resolve(__dirname, '../wwwroot/dist'),
        filename: 'main.js'
  },
  module: {
    rules: [
      {
        test: /\.vue$/,
        use: 'vue-loader'
      }
    ]
  },
  plugins: [
    new VueLoaderPlugin()
  ]
}
