package xxAMIDOxx.xxSTACKSxx.API.PerformanceTests.Gatling.requests

import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.http.request.builder.HttpRequestBuilder
import xxAMIDOxx.xxSTACKSxx.API.PerformanceTests.Gatling.config.Config._

//Requests represent the REST requests
object SearchMenuRequest {
  var get_menu: HttpRequestBuilder = http("Get Menu")
    .get(baseUrl + "/v1/menu?search=test")
    .check(status is 204)
}
