package com.amido.requests


import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.http.request.builder.HttpRequestBuilder
import com.amido.config.Configuration.baseUrl

object CreatelocalhostRequest {
  var create_localhost: HttpRequestBuilder = http("Create localhost")
    .post(baseUrl + "/v1/localhost")
    .body(RawFileBody("./src/test/resources/bodies/Createlocalhost.json")).asJson
    .header("content-type", "application/json")
    .check(status is 201)
}
