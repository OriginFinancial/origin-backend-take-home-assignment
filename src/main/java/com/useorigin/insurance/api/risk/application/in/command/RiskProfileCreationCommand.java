package com.useorigin.insurance.api.risk.application.in.command;

import com.useorigin.insurance.api.risk.domain.House;
import com.useorigin.insurance.api.risk.domain.Vehicle;
import com.useorigin.insurance.api.risk.domain.command.MaritalStatus;

public class RiskProfileCreationCommand {

    private final int age;
    private final int dependents;
    private final House house;
    private final int income;
    private final MaritalStatus maritalStatus;
    private final boolean[] riskQuestions;
    private final Vehicle vehicle;

    public RiskProfileCreationCommand(int age, int dependents, House house, int income, MaritalStatus maritalStatus, boolean[] riskQuestions, Vehicle vehicle) {
        this.age = age;
        this.dependents = dependents;
        this.house = house;
        this.income = income;
        this.maritalStatus = maritalStatus;
        this.riskQuestions = riskQuestions;
        this.vehicle = vehicle;
    }

    public static class Builder {
        private int age;
        private int dependents;
        private House house;
        private int income;
        private MaritalStatus maritalStatus;
        private boolean[] riskQuestions;
        private Vehicle vehicle;

        public Builder atAge(int age) {
            this.age = age;
            return this;
        }

        public Builder withDependents(int dependents) {
            this.dependents = dependents;
            return this;
        }

        public Builder withHouse(House house) {
            this.house = house;
            return this;
        }

        public Builder withIncome(int income) {
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
