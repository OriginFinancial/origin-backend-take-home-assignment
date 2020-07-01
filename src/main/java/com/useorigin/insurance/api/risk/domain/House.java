package com.useorigin.insurance.api.risk.domain;

import com.fasterxml.jackson.databind.PropertyNamingStrategy;
import com.fasterxml.jackson.databind.annotation.JsonNaming;

@JsonNaming(PropertyNamingStrategy.SnakeCaseStrategy.class)
public class House {

    private OwnershipStatus ownershipStatus;

    public House() {
    }

    public House(OwnershipStatus ownershipStatus) {
        this.ownershipStatus = ownershipStatus;
    }

    public OwnershipStatus getOwnershipStatus() {
        return ownershipStatus;
    }

    @Override
    public String toString() {
        return "House{" +
                "ownershipStatus=" + ownershipStatus +
                '}';
    }
}
