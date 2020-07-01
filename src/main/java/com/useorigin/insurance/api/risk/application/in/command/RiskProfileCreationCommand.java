package com.useorigin.insurance.api.risk.application.in.command;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.databind.PropertyNamingStrategy;
import com.fasterxml.jackson.databind.annotation.JsonNaming;
import com.useorigin.insurance.api.risk.domain.House;
import com.useorigin.insurance.api.risk.domain.Vehicle;
import com.useorigin.insurance.api.risk.domain.command.MaritalStatus;

import javax.validation.constraints.Min;
import javax.validation.constraints.NotNull;
import javax.validation.constraints.Size;

@JsonNaming(PropertyNamingStrategy.SnakeCaseStrategy.class)
public class RiskProfileCreationCommand {

    @NotNull
    @Min(0)
    private final Integer age;

    @Min(0)
    private final Integer dependents;
    private final House house;

    @Min(0)
    private final Integer income;

    @JsonProperty(value = "marital_status")
    @NotNull
    private final MaritalStatus maritalStatus;

    @NotNull
    @JsonProperty(value = "risk_questions")
    @Size(min = 3, max = 3)
    private final boolean[] riskQuestions;
    private final Vehicle vehicle;

    public RiskProfileCreationCommand(Integer age, Integer dependents, House house, Integer income, MaritalStatus maritalStatus, boolean[] riskQuestions, Vehicle vehicle) {
        this.age = age;
        this.dependents = dependents;
        this.house = house;
        this.income = income;
        this.maritalStatus = maritalStatus;
        this.riskQuestions = riskQuestions;
        this.vehicle = vehicle;
    }

    public static class Builder {
        private Integer age;
        private Integer dependents;
        private House house;
        private Integer income;
        private MaritalStatus maritalStatus;
        private boolean[] riskQuestions;
        private Vehicle vehicle;

        public Builder atAge(Integer age) {
            this.age = age;
            return this;
        }

        public Builder withDependents(Integer dependents) {
            this.dependents = dependents;
            return this;
        }

        public Builder withHouse(House house) {
            this.house = house;
            return this;
        }

        public Builder withIncome(Integer income) {
            this.income = income;
            return this;
        }

        public Builder withMaritalStatus(MaritalStatus maritalStatus) {
            this.maritalStatus = maritalStatus;
            return this;
        }

        public Builder withRisks(boolean[] riskQuestions) {
            this.riskQuestions = riskQuestions;
            return this;
        }

        public Builder withCar(Vehicle vehicle) {
            this.vehicle = vehicle;
            return this;
        }

        public RiskProfileCreationCommand build() {
            return new RiskProfileCreationCommand(age, dependents, house, income, maritalStatus, riskQuestions, vehicle);
        }

    }
}