import { getGreeting, getMenuButton, getTemplatesButton } from "../support/app.po";

describe("jerry-app", () => {
    beforeEach(() => cy.visit("/"));

    it("should navigate to job templates", () => {
        getGreeting().contains("Welcome to RUF!");
        getMenuButton().click();
        getTemplatesButton().click();
        getGreeting().contains("Job Templates");
    });
});
