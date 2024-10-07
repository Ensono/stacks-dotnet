FROM golang:1.23.2 as build
WORKDIR /go-app
COPY ./ ./
RUN CGO_ENABLED=0 go build -o tester .

FROM gcr.io/distroless/static-debian12
COPY --from=build /go-app/tester /
CMD ["/tester"]
