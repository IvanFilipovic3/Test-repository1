module.exports = {
    name: "jerry-app",
    preset: "../../jest.config.js",
    coverageDirectory: "../../coverage/apps/jerry-app",
    snapshotSerializers: [
        "jest-preset-angular/AngularSnapshotSerializer.js",
        "jest-preset-angular/HTMLCommentSerializer.js"
    ],
    moduleNameMapper: {
        "^shared/(.*)$": "<rootDir>/src/app/shared/$1", // add new mapping
        "^client-proxies/(.*)$": "<rootDir>/src/app/client-proxies/$1" // add new mapping
    },
    reporters: [
        "default",
        ["jest-html-reporters", {
            "publicPath": "./html-report/apps/jerry-app"
        }]
    ]
};
