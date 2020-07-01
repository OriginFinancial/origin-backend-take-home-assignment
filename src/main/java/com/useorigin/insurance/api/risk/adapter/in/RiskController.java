package com.useorigin.insurance.api.risk.adapter.in;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

import javax.validation.Valid;

import static org.springframework.http.HttpStatus.CREATED;

@RestController
public class RiskController {

    @PostMapping("/insurances/risk")
    private ResponseEntity<RiskProfileResource> risk(@RequestBody @Valid RiskProfileCreationCommand command) {


        RiskProfileResource riskProfileResource = new RiskProfileResource("regular", "ineligible", "economic", "regular");
        return ResponseEntity.status(CREATED).body(riskProfileResource);
    }
}
