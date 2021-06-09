package com.amido.requests

import com.amido.config.Configuration.baseUrl
import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.http.request.builder.HttpRequestBuilder

//Requests represent the REST requests
object GetlocalhostRequest {
  var get_localhost: HttpRequestBuilder = http("Get localhost")
    .get(baseUrl + "/v1/localhost?search=test")
    .check(status is 200)
}
