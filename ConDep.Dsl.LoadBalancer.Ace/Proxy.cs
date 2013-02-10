namespace ConDep.Dsl.LoadBalancer.Ace.Proxy
{
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class WSException
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string message;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class SessionToken
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sid;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class loginResponse
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SessionToken SessionToken;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class login
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string user;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string password;
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://anm.cisco.com", ConfigurationName="IOperationManager")]
    public interface IOperationManager
    {
        
        // CODEGEN: Generating message contract since the operation login is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(WSException), Action="", Name="WSException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        loginResponse1 login(loginRequest request);
        
        // CODEGEN: Generating message contract since the operation logout is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(WSException), Action="", Name="WSException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        logoutResponse1 logout(logoutRequest request);
        
        // CODEGEN: Generating message contract since the operation listServerFarms is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(WSException), Action="", Name="WSException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        listServerFarmsResponse1 listServerFarms(listServerFarmsRequest request);
        
        // CODEGEN: Generating message contract since the operation listRservers is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(WSException), Action="", Name="WSException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        listRserversResponse1 listRservers(listRserversRequest request);
        
        // CODEGEN: Generating message contract since the operation listServerfarmRservers is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(WSException), Action="", Name="WSException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        listServerfarmRserversResponse1 listServerfarmRservers(listServerfarmRserversRequest request);
        
        // CODEGEN: Generating message contract since the operation addRserverToServerfarm is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(WSException), Action="", Name="WSException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        addRserverToServerfarmResponse1 addRserverToServerfarm(addRserverToServerfarmRequest request);
        
        // CODEGEN: Generating message contract since the operation removeRserverFromServerfarm is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(WSException), Action="", Name="WSException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        removeRserverFromServerfarmResponse1 removeRserverFromServerfarm(removeRserverFromServerfarmRequest request);
        
        // CODEGEN: Generating message contract since the operation listDeviceIds is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(WSException), Action="", Name="WSException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        listDeviceIdsResponse1 listDeviceIds(listDeviceIdsRequest request);
        
        // CODEGEN: Generating message contract since the operation listVirtualContexts is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(WSException), Action="", Name="WSException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        listVirtualContextsResponse1 listVirtualContexts(listVirtualContextsRequest request);
        
        // CODEGEN: Generating message contract since the operation activateServerfarmRserver is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(WSException), Action="", Name="WSException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        activateServerfarmRserverResponse1 activateServerfarmRserver(activateServerfarmRserverRequest request);
        
        // CODEGEN: Generating message contract since the operation suspendServerfarmRserver is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(WSException), Action="", Name="WSException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        suspendServerfarmRserverResponse1 suspendServerfarmRserver(suspendServerfarmRserverRequest request);
        
        // CODEGEN: Generating message contract since the operation changeServerfarmRserverWeight is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(WSException), Action="", Name="WSException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        changeServerfarmRserverWeightResponse1 changeServerfarmRserverWeight(changeServerfarmRserverWeightRequest request);
        
        // CODEGEN: Generating message contract since the operation getVMMappingInfo is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(WSException), Action="", Name="WSException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        getVMMappingInfoResponse1 getVMMappingInfo(getVMMappingInfoRequest request);
        
        // CODEGEN: Generating message contract since the operation isEmptyOrNull is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        isEmptyOrNullResponse1 isEmptyOrNull(isEmptyOrNullRequest request);
        
        // CODEGEN: Generating message contract since the operation validateIPAddress is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(WSException), Action="", Name="WSException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        validateIPAddressResponse1 validateIPAddress(validateIPAddressRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class loginRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public login login;
        
        public loginRequest()
        {
        }
        
        public loginRequest(login login)
        {
            this.login = login;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class loginResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public loginResponse loginResponse;
        
        public loginResponse1()
        {
        }
        
        public loginResponse1(loginResponse loginResponse)
        {
            this.loginResponse = loginResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class logout
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SessionToken sessionToken;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class logoutResponse
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class logoutRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public logout logout;
        
        public logoutRequest()
        {
        }
        
        public logoutRequest(logout logout)
        {
            this.logout = logout;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class logoutResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public logoutResponse logoutResponse;
        
        public logoutResponse1()
        {
        }
        
        public logoutResponse1(logoutResponse logoutResponse)
        {
            this.logoutResponse = logoutResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class listServerFarms
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SessionToken sessionToken;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DeviceID deviceID;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class DeviceID
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DeviceType deviceType;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deviceTypeSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ipAddr;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string slot;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string chassisIPAddr;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public enum DeviceType
    {
        
        /// <remarks/>
        CAT6K,
        
        /// <remarks/>
        ACE_BLADE,
        
        /// <remarks/>
        VIRTUAL_CONTEXT,
        
        /// <remarks/>
        ACE4710,
        
        /// <remarks/>
        GSS,
        
        /// <remarks/>
        CSS,
        
        /// <remarks/>
        CSM,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class listServerFarmsResponse
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Serverfarm[] serverfarms;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class Serverfarm
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public serverFarmType serverFarmType;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public enum serverFarmType
    {
        
        /// <remarks/>
        host,
        
        /// <remarks/>
        redirect,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class listServerFarmsRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public listServerFarms listServerFarms;
        
        public listServerFarmsRequest()
        {
        }
        
        public listServerFarmsRequest(listServerFarms listServerFarms)
        {
            this.listServerFarms = listServerFarms;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class listServerFarmsResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public listServerFarmsResponse listServerFarmsResponse;
        
        public listServerFarmsResponse1()
        {
        }
        
        public listServerFarmsResponse1(listServerFarmsResponse listServerFarmsResponse)
        {
            this.listServerFarmsResponse = listServerFarmsResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class listRservers
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SessionToken sessionToken;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DeviceID deviceID;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class listRserversResponse
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Rserver[] Rservers;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class Rserver
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public type type;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ipAddr;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public state state;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int weight;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public enum type
    {
        
        /// <remarks/>
        host,
        
        /// <remarks/>
        redirect,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public enum state
    {
        
        /// <remarks/>
        IS,
        
        /// <remarks/>
        OOS,
        
        /// <remarks/>
        ISS,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class listRserversRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public listRservers listRservers;
        
        public listRserversRequest()
        {
        }
        
        public listRserversRequest(listRservers listRservers)
        {
            this.listRservers = listRservers;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class listRserversResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public listRserversResponse listRserversResponse;
        
        public listRserversResponse1()
        {
        }
        
        public listRserversResponse1(listRserversResponse listRserversResponse)
        {
            this.listRserversResponse = listRserversResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class listServerfarmRservers
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SessionToken sessionToken;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DeviceID deviceID;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serverfarmname;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class listServerfarmRserversResponse
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SfRserver[] SfRservers;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class SfRserver
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serverfarmName;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string realserverName;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public realServerStates adminState;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int weight;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool weightSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int port;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ipAddr;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public enum realServerStates
    {
        
        /// <remarks/>
        IS,
        
        /// <remarks/>
        OOS,
        
        /// <remarks/>
        ISS,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class listServerfarmRserversRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public listServerfarmRservers listServerfarmRservers;
        
        public listServerfarmRserversRequest()
        {
        }
        
        public listServerfarmRserversRequest(listServerfarmRservers listServerfarmRservers)
        {
            this.listServerfarmRservers = listServerfarmRservers;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class listServerfarmRserversResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public listServerfarmRserversResponse listServerfarmRserversResponse;
        
        public listServerfarmRserversResponse1()
        {
        }
        
        public listServerfarmRserversResponse1(listServerfarmRserversResponse listServerfarmRserversResponse)
        {
            this.listServerfarmRserversResponse = listServerfarmRserversResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class addRserverToServerfarm
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SessionToken sessionToken;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DeviceID deviceID;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serverfarmname;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Rserver rserver;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int port;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class addRserverToServerfarmResponse
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class addRserverToServerfarmRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public addRserverToServerfarm addRserverToServerfarm;
        
        public addRserverToServerfarmRequest()
        {
        }
        
        public addRserverToServerfarmRequest(addRserverToServerfarm addRserverToServerfarm)
        {
            this.addRserverToServerfarm = addRserverToServerfarm;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class addRserverToServerfarmResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public addRserverToServerfarmResponse addRserverToServerfarmResponse;
        
        public addRserverToServerfarmResponse1()
        {
        }
        
        public addRserverToServerfarmResponse1(addRserverToServerfarmResponse addRserverToServerfarmResponse)
        {
            this.addRserverToServerfarmResponse = addRserverToServerfarmResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class removeRserverFromServerfarm
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SessionToken sessionToken;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DeviceID deviceID;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SfRserver rserver;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class removeRserverFromServerfarmResponse
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class removeRserverFromServerfarmRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public removeRserverFromServerfarm removeRserverFromServerfarm;
        
        public removeRserverFromServerfarmRequest()
        {
        }
        
        public removeRserverFromServerfarmRequest(removeRserverFromServerfarm removeRserverFromServerfarm)
        {
            this.removeRserverFromServerfarm = removeRserverFromServerfarm;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class removeRserverFromServerfarmResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public removeRserverFromServerfarmResponse removeRserverFromServerfarmResponse;
        
        public removeRserverFromServerfarmResponse1()
        {
        }
        
        public removeRserverFromServerfarmResponse1(removeRserverFromServerfarmResponse removeRserverFromServerfarmResponse)
        {
            this.removeRserverFromServerfarmResponse = removeRserverFromServerfarmResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class listDeviceIds
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SessionToken sessionToken;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DeviceType deviceType;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class listDeviceIdsResponse
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DeviceID[] DeviceIDs;
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class listDeviceIdsRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public listDeviceIds listDeviceIds;
        
        public listDeviceIdsRequest()
        {
        }
        
        public listDeviceIdsRequest(listDeviceIds listDeviceIds)
        {
            this.listDeviceIds = listDeviceIds;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class listDeviceIdsResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public listDeviceIdsResponse listDeviceIdsResponse;
        
        public listDeviceIdsResponse1()
        {
        }
        
        public listDeviceIdsResponse1(listDeviceIdsResponse listDeviceIdsResponse)
        {
            this.listDeviceIdsResponse = listDeviceIdsResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class listVirtualContexts
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SessionToken sessionToken;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DeviceID deviceIDs;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class listVirtualContextsResponse
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DeviceID[] DeviceIDs;
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class listVirtualContextsRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public listVirtualContexts listVirtualContexts;
        
        public listVirtualContextsRequest()
        {
        }
        
        public listVirtualContextsRequest(listVirtualContexts listVirtualContexts)
        {
            this.listVirtualContexts = listVirtualContexts;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class listVirtualContextsResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public listVirtualContextsResponse listVirtualContextsResponse;
        
        public listVirtualContextsResponse1()
        {
        }
        
        public listVirtualContextsResponse1(listVirtualContextsResponse listVirtualContextsResponse)
        {
            this.listVirtualContextsResponse = listVirtualContextsResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class activateServerfarmRserver
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SessionToken sessionToken;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DeviceID deviceID;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SfRserver rserver;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string reason;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class activateServerfarmRserverResponse
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class activateServerfarmRserverRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public activateServerfarmRserver activateServerfarmRserver;
        
        public activateServerfarmRserverRequest()
        {
        }
        
        public activateServerfarmRserverRequest(activateServerfarmRserver activateServerfarmRserver)
        {
            this.activateServerfarmRserver = activateServerfarmRserver;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class activateServerfarmRserverResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public activateServerfarmRserverResponse activateServerfarmRserverResponse;
        
        public activateServerfarmRserverResponse1()
        {
        }
        
        public activateServerfarmRserverResponse1(activateServerfarmRserverResponse activateServerfarmRserverResponse)
        {
            this.activateServerfarmRserverResponse = activateServerfarmRserverResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class suspendServerfarmRserver
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SessionToken sessionToken;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DeviceID deviceID;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SfRserver rserver;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SuspendState suspendState;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string reason;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public enum SuspendState
    {
        
        /// <remarks/>
        Graceful,
        
        /// <remarks/>
        Suspend,
        
        /// <remarks/>
        Suspend_Clear_Connections,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class suspendServerfarmRserverResponse
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class suspendServerfarmRserverRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public suspendServerfarmRserver suspendServerfarmRserver;
        
        public suspendServerfarmRserverRequest()
        {
        }
        
        public suspendServerfarmRserverRequest(suspendServerfarmRserver suspendServerfarmRserver)
        {
            this.suspendServerfarmRserver = suspendServerfarmRserver;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class suspendServerfarmRserverResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public suspendServerfarmRserverResponse suspendServerfarmRserverResponse;
        
        public suspendServerfarmRserverResponse1()
        {
        }
        
        public suspendServerfarmRserverResponse1(suspendServerfarmRserverResponse suspendServerfarmRserverResponse)
        {
            this.suspendServerfarmRserverResponse = suspendServerfarmRserverResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class changeServerfarmRserverWeight
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SessionToken sessionToken;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DeviceID deviceID;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SfRserver rserver;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string reason;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class changeServerfarmRserverWeightResponse
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class changeServerfarmRserverWeightRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public changeServerfarmRserverWeight changeServerfarmRserverWeight;
        
        public changeServerfarmRserverWeightRequest()
        {
        }
        
        public changeServerfarmRserverWeightRequest(changeServerfarmRserverWeight changeServerfarmRserverWeight)
        {
            this.changeServerfarmRserverWeight = changeServerfarmRserverWeight;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class changeServerfarmRserverWeightResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public changeServerfarmRserverWeightResponse changeServerfarmRserverWeightResponse;
        
        public changeServerfarmRserverWeightResponse1()
        {
        }
        
        public changeServerfarmRserverWeightResponse1(changeServerfarmRserverWeightResponse changeServerfarmRserverWeightResponse)
        {
            this.changeServerfarmRserverWeightResponse = changeServerfarmRserverWeightResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class getVMMappingInfo
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SessionToken sessionToken;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string vmname;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class getVMMappingInfoResponse
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public VMMappingInfo[] VMMappingInfos;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class VMMappingInfo
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DeviceID deviceID;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sfname;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sfrsname;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int port;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool portSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ipaddr;
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getVMMappingInfoRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public getVMMappingInfo getVMMappingInfo;
        
        public getVMMappingInfoRequest()
        {
        }
        
        public getVMMappingInfoRequest(getVMMappingInfo getVMMappingInfo)
        {
            this.getVMMappingInfo = getVMMappingInfo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getVMMappingInfoResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public getVMMappingInfoResponse getVMMappingInfoResponse;
        
        public getVMMappingInfoResponse1()
        {
        }
        
        public getVMMappingInfoResponse1(getVMMappingInfoResponse getVMMappingInfoResponse)
        {
            this.getVMMappingInfoResponse = getVMMappingInfoResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class isEmptyOrNull
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arg0;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class isEmptyOrNullResponse
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool @return;
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class isEmptyOrNullRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public isEmptyOrNull isEmptyOrNull;
        
        public isEmptyOrNullRequest()
        {
        }
        
        public isEmptyOrNullRequest(isEmptyOrNull isEmptyOrNull)
        {
            this.isEmptyOrNull = isEmptyOrNull;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class isEmptyOrNullResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public isEmptyOrNullResponse isEmptyOrNullResponse;
        
        public isEmptyOrNullResponse1()
        {
        }
        
        public isEmptyOrNullResponse1(isEmptyOrNullResponse isEmptyOrNullResponse)
        {
            this.isEmptyOrNullResponse = isEmptyOrNullResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class validateIPAddress
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arg0;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://anm.cisco.com")]
    public partial class validateIPAddressResponse
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class validateIPAddressRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public validateIPAddress validateIPAddress;
        
        public validateIPAddressRequest()
        {
        }
        
        public validateIPAddressRequest(validateIPAddress validateIPAddress)
        {
            this.validateIPAddress = validateIPAddress;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class validateIPAddressResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://anm.cisco.com", Order=0)]
        public validateIPAddressResponse validateIPAddressResponse;
        
        public validateIPAddressResponse1()
        {
        }
        
        public validateIPAddressResponse1(validateIPAddressResponse validateIPAddressResponse)
        {
            this.validateIPAddressResponse = validateIPAddressResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IOperationManagerChannel : IOperationManager, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class OperationManagerClient : System.ServiceModel.ClientBase<IOperationManager>, IOperationManager
    {
        
        public OperationManagerClient()
        {
        }
        
        public OperationManagerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName)
        {
        }
        
        public OperationManagerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public OperationManagerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public OperationManagerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        loginResponse1 IOperationManager.login(loginRequest request)
        {
            return base.Channel.login(request);
        }
        
        public loginResponse login(login login1)
        {
            loginRequest inValue = new loginRequest();
            inValue.login = login1;
            loginResponse1 retVal = ((IOperationManager)(this)).login(inValue);
            return retVal.loginResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        logoutResponse1 IOperationManager.logout(logoutRequest request)
        {
            return base.Channel.logout(request);
        }
        
        public logoutResponse logout(logout logout1)
        {
            logoutRequest inValue = new logoutRequest();
            inValue.logout = logout1;
            logoutResponse1 retVal = ((IOperationManager)(this)).logout(inValue);
            return retVal.logoutResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        listServerFarmsResponse1 IOperationManager.listServerFarms(listServerFarmsRequest request)
        {
            return base.Channel.listServerFarms(request);
        }
        
        public listServerFarmsResponse listServerFarms(listServerFarms listServerFarms1)
        {
            listServerFarmsRequest inValue = new listServerFarmsRequest();
            inValue.listServerFarms = listServerFarms1;
            listServerFarmsResponse1 retVal = ((IOperationManager)(this)).listServerFarms(inValue);
            return retVal.listServerFarmsResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        listRserversResponse1 IOperationManager.listRservers(listRserversRequest request)
        {
            return base.Channel.listRservers(request);
        }
        
        public listRserversResponse listRservers(listRservers listRservers1)
        {
            listRserversRequest inValue = new listRserversRequest();
            inValue.listRservers = listRservers1;
            listRserversResponse1 retVal = ((IOperationManager)(this)).listRservers(inValue);
            return retVal.listRserversResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        listServerfarmRserversResponse1 IOperationManager.listServerfarmRservers(listServerfarmRserversRequest request)
        {
            return base.Channel.listServerfarmRservers(request);
        }
        
        public listServerfarmRserversResponse listServerfarmRservers(listServerfarmRservers listServerfarmRservers1)
        {
            listServerfarmRserversRequest inValue = new listServerfarmRserversRequest();
            inValue.listServerfarmRservers = listServerfarmRservers1;
            listServerfarmRserversResponse1 retVal = ((IOperationManager)(this)).listServerfarmRservers(inValue);
            return retVal.listServerfarmRserversResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        addRserverToServerfarmResponse1 IOperationManager.addRserverToServerfarm(addRserverToServerfarmRequest request)
        {
            return base.Channel.addRserverToServerfarm(request);
        }
        
        public addRserverToServerfarmResponse addRserverToServerfarm(addRserverToServerfarm addRserverToServerfarm1)
        {
            addRserverToServerfarmRequest inValue = new addRserverToServerfarmRequest();
            inValue.addRserverToServerfarm = addRserverToServerfarm1;
            addRserverToServerfarmResponse1 retVal = ((IOperationManager)(this)).addRserverToServerfarm(inValue);
            return retVal.addRserverToServerfarmResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        removeRserverFromServerfarmResponse1 IOperationManager.removeRserverFromServerfarm(removeRserverFromServerfarmRequest request)
        {
            return base.Channel.removeRserverFromServerfarm(request);
        }
        
        public removeRserverFromServerfarmResponse removeRserverFromServerfarm(removeRserverFromServerfarm removeRserverFromServerfarm1)
        {
            removeRserverFromServerfarmRequest inValue = new removeRserverFromServerfarmRequest();
            inValue.removeRserverFromServerfarm = removeRserverFromServerfarm1;
            removeRserverFromServerfarmResponse1 retVal = ((IOperationManager)(this)).removeRserverFromServerfarm(inValue);
            return retVal.removeRserverFromServerfarmResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        listDeviceIdsResponse1 IOperationManager.listDeviceIds(listDeviceIdsRequest request)
        {
            return base.Channel.listDeviceIds(request);
        }
        
        public listDeviceIdsResponse listDeviceIds(listDeviceIds listDeviceIds1)
        {
            listDeviceIdsRequest inValue = new listDeviceIdsRequest();
            inValue.listDeviceIds = listDeviceIds1;
            listDeviceIdsResponse1 retVal = ((IOperationManager)(this)).listDeviceIds(inValue);
            return retVal.listDeviceIdsResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        listVirtualContextsResponse1 IOperationManager.listVirtualContexts(listVirtualContextsRequest request)
        {
            return base.Channel.listVirtualContexts(request);
        }
        
        public listVirtualContextsResponse listVirtualContexts(listVirtualContexts listVirtualContexts1)
        {
            listVirtualContextsRequest inValue = new listVirtualContextsRequest();
            inValue.listVirtualContexts = listVirtualContexts1;
            listVirtualContextsResponse1 retVal = ((IOperationManager)(this)).listVirtualContexts(inValue);
            return retVal.listVirtualContextsResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        activateServerfarmRserverResponse1 IOperationManager.activateServerfarmRserver(activateServerfarmRserverRequest request)
        {
            return base.Channel.activateServerfarmRserver(request);
        }
        
        public activateServerfarmRserverResponse activateServerfarmRserver(activateServerfarmRserver activateServerfarmRserver1)
        {
            activateServerfarmRserverRequest inValue = new activateServerfarmRserverRequest();
            inValue.activateServerfarmRserver = activateServerfarmRserver1;
            activateServerfarmRserverResponse1 retVal = ((IOperationManager)(this)).activateServerfarmRserver(inValue);
            return retVal.activateServerfarmRserverResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        suspendServerfarmRserverResponse1 IOperationManager.suspendServerfarmRserver(suspendServerfarmRserverRequest request)
        {
            return base.Channel.suspendServerfarmRserver(request);
        }
        
        public suspendServerfarmRserverResponse suspendServerfarmRserver(suspendServerfarmRserver suspendServerfarmRserver1)
        {
            suspendServerfarmRserverRequest inValue = new suspendServerfarmRserverRequest();
            inValue.suspendServerfarmRserver = suspendServerfarmRserver1;
            suspendServerfarmRserverResponse1 retVal = ((IOperationManager)(this)).suspendServerfarmRserver(inValue);
            return retVal.suspendServerfarmRserverResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        changeServerfarmRserverWeightResponse1 IOperationManager.changeServerfarmRserverWeight(changeServerfarmRserverWeightRequest request)
        {
            return base.Channel.changeServerfarmRserverWeight(request);
        }
        
        public changeServerfarmRserverWeightResponse changeServerfarmRserverWeight(changeServerfarmRserverWeight changeServerfarmRserverWeight1)
        {
            changeServerfarmRserverWeightRequest inValue = new changeServerfarmRserverWeightRequest();
            inValue.changeServerfarmRserverWeight = changeServerfarmRserverWeight1;
            changeServerfarmRserverWeightResponse1 retVal = ((IOperationManager)(this)).changeServerfarmRserverWeight(inValue);
            return retVal.changeServerfarmRserverWeightResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        getVMMappingInfoResponse1 IOperationManager.getVMMappingInfo(getVMMappingInfoRequest request)
        {
            return base.Channel.getVMMappingInfo(request);
        }
        
        public getVMMappingInfoResponse getVMMappingInfo(getVMMappingInfo getVMMappingInfo1)
        {
            getVMMappingInfoRequest inValue = new getVMMappingInfoRequest();
            inValue.getVMMappingInfo = getVMMappingInfo1;
            getVMMappingInfoResponse1 retVal = ((IOperationManager)(this)).getVMMappingInfo(inValue);
            return retVal.getVMMappingInfoResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        isEmptyOrNullResponse1 IOperationManager.isEmptyOrNull(isEmptyOrNullRequest request)
        {
            return base.Channel.isEmptyOrNull(request);
        }
        
        public isEmptyOrNullResponse isEmptyOrNull(isEmptyOrNull isEmptyOrNull1)
        {
            isEmptyOrNullRequest inValue = new isEmptyOrNullRequest();
            inValue.isEmptyOrNull = isEmptyOrNull1;
            isEmptyOrNullResponse1 retVal = ((IOperationManager)(this)).isEmptyOrNull(inValue);
            return retVal.isEmptyOrNullResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        validateIPAddressResponse1 IOperationManager.validateIPAddress(validateIPAddressRequest request)
        {
            return base.Channel.validateIPAddress(request);
        }
        
        public validateIPAddressResponse validateIPAddress(validateIPAddress validateIPAddress1)
        {
            validateIPAddressRequest inValue = new validateIPAddressRequest();
            inValue.validateIPAddress = validateIPAddress1;
            validateIPAddressResponse1 retVal = ((IOperationManager)(this)).validateIPAddress(inValue);
            return retVal.validateIPAddressResponse;
        }
    }
}
